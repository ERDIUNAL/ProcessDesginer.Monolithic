using Application.Features.Auth.Rules;
using Application.Services.Core.AuthService;
using Application.Services.Core.Repositories;
using Crea.Core.Security.Entities;
using Crea.Core.Security.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.Refresh;

public class RefreshCommand:IRequest<RefreshedResponse>
{
    public string RefreshToken { get; set; }
    public string IpAddress { get; set; }

    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshedResponse>
    {
        IAuthService _authService;
        IRefreshTokenRepository _refreshTokenRepository;
        IUserRepository _userRepository;
        AuthBusinessRules _authBusinessRules;

        public RefreshCommandHandler(IAuthService authService, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, AuthBusinessRules authBusinessRules)
        {
            _authService = authService;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RefreshedResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.GetAsync(x => x.Token == request.RefreshToken);

            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

            if(refreshToken!.RevokedDate != null)
            {
                await _authService.RevokeDescendantRefreshTokens(
                    refreshToken,
                    request.IpAddress,
                    $"Geçersiz kılınmış RefreshToken kullanılmaya çalışıldı: {refreshToken.Token}");
            }

            await _authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

            User user = (await _userRepository.GetAsync(x => x.Id == refreshToken.UserId))!;

            AccessToken createdAccessToken = await _authService.CreateAccessToken(user);
            RefreshToken createdRefreshToken = await _authService.RotateRefreshToken(user, refreshToken, request.IpAddress);

            RefreshedResponse response = new()
            {
                AccessToken = createdAccessToken,
                RefreshToken = createdRefreshToken,
            };

            return response;
        }
    }
}
