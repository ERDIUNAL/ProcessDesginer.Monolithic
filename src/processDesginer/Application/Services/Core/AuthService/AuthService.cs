using Application.Services.Core.Repositories;
using Crea.Core.CrossCuttingConcerns.Exceptions;
using Crea.Core.Mailing;
using Crea.Core.Security.Authenticator;
using Crea.Core.Security.Authenticator.Email;
using Crea.Core.Security.Authenticator.Otp;
using Crea.Core.Security.Entities;
using Crea.Core.Security.JWT;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Core.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailAuthenticatorHelper _emailAuthenticatorHelper;
    private readonly IUserEmailAuthenticatorRepository _userEmailAuthenticatorRepository;
    private readonly IUserOtpAuthenticatorRepository _userOtpAuthenticatorRepository;
    private readonly IOtpAuthenticatorHelper _otpAuthenticatorHelper;
    private readonly IMailService _mailService;
    private readonly ITokenHelper _tokenHelper;

    public AuthService(IUserOperationClaimRepository userOperationClaimRepository, IRefreshTokenRepository refreshTokenRepository, IEmailAuthenticatorHelper emailAuthenticatorHelper, IUserEmailAuthenticatorRepository userEmailAuthenticatorRepository, IUserOtpAuthenticatorRepository userOtpAuthenticatorRepository, IOtpAuthenticatorHelper otpAuthenticatorHelper, IMailService mailService, ITokenHelper tokenHelper)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _emailAuthenticatorHelper = emailAuthenticatorHelper;
        _userEmailAuthenticatorRepository = userEmailAuthenticatorRepository;
        _userOtpAuthenticatorRepository = userOtpAuthenticatorRepository;
        _otpAuthenticatorHelper = otpAuthenticatorHelper;
        _mailService = mailService;
        _tokenHelper = tokenHelper;
    }

    public async Task<AccessToken> CreateAccessToken(User user)
    {
        var operationClaims = await _userOperationClaimRepository.GetOperationClaimsByUserIdAsync(user.Id);
        var accessToken = _tokenHelper.CreateToken(user, operationClaims);

        return accessToken;
    }

    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);

        return Task.FromResult(refreshToken);
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);

        return addedRefreshToken;
    }

    public async Task DeleteOldActiveRefreshTokens(User user)
    {
        ICollection<RefreshToken> oldActiveRefreshTokens =
            await _refreshTokenRepository.GetAllOldActiveRefreshTokensAsync(user, _tokenHelper.RefreshTokenTTLOption);

        await _refreshTokenRepository.DeleteRangeAsync(oldActiveRefreshTokens.ToList());
    }

    public async Task RevokeDescendantRefreshTokens(RefreshToken token, string ipAddress, string reason)
    {
        RefreshToken? childRefreshToken =
            await _refreshTokenRepository.GetAsync(x => x.Token == token.ReplacedByToken);

        if (childRefreshToken == null)
        {
            throw new BusinessException("Kullanılmaya çalışılan Refresh Token'ın child bulunamadı");
        }

        if (childRefreshToken.RevokedDate == null)
        {
            await RevokeRefreshToken(childRefreshToken, ipAddress, reason);
        }
        else
        {
            await RevokeDescendantRefreshTokens(childRefreshToken, ipAddress, reason);
        }
    }

    public async Task RevokeRefreshToken(RefreshToken token, string ipAddress, string reason, string? replacedByToken = default)
    {
        token.RevokedDate = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.RevokedReason = reason;
        token.ReplacedByToken = replacedByToken;

        await _refreshTokenRepository.UpdateAsync(token);
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken token, string ipAddress)
    {
        RefreshToken newRefreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);

        await RevokeRefreshToken(token, ipAddress, "Yeni RefreshToken oluşturuldu", newRefreshToken.Token);
        await AddRefreshToken(newRefreshToken);

        return newRefreshToken;
    }

    public async Task<UserEmailAuthenticator> CreateEmailAuthenticator(User user) =>
        new UserEmailAuthenticator
        {
            UserId = user.Id,
            Key = await _emailAuthenticatorHelper.CreateEmailActivationKeyAsync(),
            IsVerified = false
        };

    public async Task<UserOtpAuthenticator> CreateOtpAuthenticator(User user)
        => new()
        {
            UserId = user.Id,
            SecretKey = await _otpAuthenticatorHelper.GenerateSecretKeyAsync(),
            IsVerified = false
        };

    public Task<string> ConvertOtpSecretKeyToString(byte[] secretKeyBytes) 
        => _otpAuthenticatorHelper.ConvertSecretKeyToStringAsync(secretKeyBytes);



    public Task SendAuthenticatorCode(User user)
    {
        return user.AuthenticatorType switch
        {
            //AuthenticatorType.None => throw new NotImplementedException(),
            AuthenticatorType.Email => sendAuthenticatorCodeWithEmail(user),
            //AuthenticatorType.Otp => throw new NotImplementedException(),
            //_ => throw new ArgumentException()
        };
    }

    public Task VerifyAuthenticatorCode(User user, string code)
    {
        return user.AuthenticatorType switch
        {
            AuthenticatorType.None => throw new NotImplementedException(),
            AuthenticatorType.Email => verifyAuthenticatorCodeWithEmail(user, code),
            AuthenticatorType.Otp => verifyAuthenticatorCodeWithOtp(user, code),
            _ => throw new ArgumentException()
        };
    }



    private async Task sendAuthenticatorCodeWithEmail(User user)
    {
        UserEmailAuthenticator userEmailAuthenticator = await _userEmailAuthenticatorRepository.GetAsync(x => x.UserId == user.Id);

        string authenticatorCode = await _emailAuthenticatorHelper.CreateEmailAuthenticatorCodeAsync();
        userEmailAuthenticator.Key = authenticatorCode;
        await _userEmailAuthenticatorRepository.UpdateAsync(userEmailAuthenticator);

        Mail mailData = new()
        {
            ToFullName = $"{user.FirstName} {user.LastName}",
            ToEmail = user.Email,
            Subject = AuthServiceBusinessMessages.AuthenticatorCodeSubject,
            TextBody = AuthServiceBusinessMessages.AuthenticatorCodeTextBody(authenticatorCode)
        };

        await _mailService.SendAsync(mailData);
    }

    private async Task verifyAuthenticatorCodeWithEmail(User user, string authenticatorCode)
    {
        UserEmailAuthenticator userEmailAuthenticator = await _userEmailAuthenticatorRepository.GetAsync(x => x.UserId == user.Id);

        if (userEmailAuthenticator.Key != authenticatorCode)
        {
            throw new BusinessException(AuthServiceBusinessMessages.InvalidAuthenticatorCode);
        }

        userEmailAuthenticator.Key = null;

        await _userEmailAuthenticatorRepository.UpdateAsync(userEmailAuthenticator);
    }

    private async Task verifyAuthenticatorCodeWithOtp(User user, string code)
    {
        UserOtpAuthenticator userOtpAuthenticator = await _userOtpAuthenticatorRepository.GetAsync(x => x.UserId == user.Id);

        bool result = await _otpAuthenticatorHelper.VerifyCodeAsync(userOtpAuthenticator.SecretKey, code);

        if(!result)
        {
            throw new BusinessException(AuthServiceBusinessMessages.InvalidAuthenticatorCode);
        }
    }
}
