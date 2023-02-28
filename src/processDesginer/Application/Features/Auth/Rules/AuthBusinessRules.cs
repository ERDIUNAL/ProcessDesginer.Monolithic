using Application.Features.Auth.Constans;
using Application.Services.Core.Repositories;
using Crea.Core.Application.Rules;
using Crea.Core.CrossCuttingConcerns.Exceptions;
using Crea.Core.Security.Authenticator;
using Crea.Core.Security.Entities;
using Crea.Core.Security.Hashing;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules : BaseBusinessRules
{
    private IUserRepository _userRepository;

    public AuthBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task UserEmailCannotBeDuplicatedWhenInserted(string email)
    {
        User? user = await _userRepository.GetAsync(u => u.Email == email);

        if (user != null)
        {
            throw new BusinessException(AuthBusinessMessages.UserEmailAlreadyExists);
        }
    }

    public Task UserShouldBeExists(User? user)
    {
        if (user == null)
        {
            throw new BusinessException(AuthBusinessMessages.UserNotFound);
        }

        return Task.CompletedTask;
    }

    public Task UserPasswordShouldBeMatch(User user, string password)
    {
        bool isMatched = HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

        if (isMatched == false)
        {
            throw new BusinessException(AuthBusinessMessages.UserPasswordNotMatch);
        }

        return Task.CompletedTask;
    }

    public Task RefreshTokenShouldBeExists(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
        {
            throw new BusinessException(AuthBusinessMessages.RefreshTokenNotFound);
        }

        return Task.CompletedTask;
    }

    public Task RefreshTokenShouldBeActive(RefreshToken refreshToken)
    {
        if (refreshToken.RevokedDate != null)
        {
            throw new BusinessException(AuthBusinessMessages.RefreshTokenNotActive);
        }

        if (refreshToken.RevokedDate == null && refreshToken.ExpiresDate < DateTime.UtcNow)
        {
            throw new BusinessException(AuthBusinessMessages.RefreshTokenNotActive);
        }

        return Task.CompletedTask;
    }

    public Task UserShouldNotHasAuthenticator(User user)
    {
        if(user.AuthenticatorType is not AuthenticatorType.None)
        {
            throw new BusinessException(AuthBusinessMessages.UserAlreadyHasAuthenticator);
        }

        return Task.CompletedTask;
    }

    public Task UserEmailAuthenticatorShouldBeExists(UserEmailAuthenticator userEmailAuthenticator)
    {
        if(userEmailAuthenticator is null)
        {
            throw new BusinessException(AuthBusinessMessages.UserEmailAuthenticatorNotFound);
        }

        return Task.CompletedTask;
    }

    public Task UserOtpAuthenticatorShouldBeExists(UserOtpAuthenticator? userOtpAuthenticator)
    {
        if (userOtpAuthenticator is null)
        {
            throw new BusinessException(AuthBusinessMessages.UserOtpAuthenticatorNotFound);
        }

        return Task.CompletedTask;
    }
}
