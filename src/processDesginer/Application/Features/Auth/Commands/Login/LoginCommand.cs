using Application.Features.Auth.Rules;
using Application.Services.Core.AuthService;
using Application.Services.Core.Repositories;
using Crea.Core.Application.Dtos;
using Crea.Core.Security.Authenticator;
using Crea.Core.Security.Entities;
using Crea.Core.Security.JWT;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoggedResponse>
{
    public UserForLoginDto UserForLoginDto { get; set; }
    public string IpAddress { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private IUserRepository _userRepository;
        private IAuthService _authService;

        public LoginCommandHandler(AuthBusinessRules authBusinessRules, IUserRepository userRepository, IAuthService authService)
        {
            _authBusinessRules = authBusinessRules;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<LoggedResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(x => x.Email == request.UserForLoginDto.Email);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserPasswordShouldBeMatch(user!, request.UserForLoginDto.Password);

            LoggedResponse response = new();

            if (user!.AuthenticatorType is not AuthenticatorType.None)
            {
                if (request.UserForLoginDto.AuthenticatorCode is null)
                {
                    await _authService.SendAuthenticatorCode(user);

                    response.RequiredAuthenticatorType = user.AuthenticatorType;

                    return response;
                }
                else
                {
                    await _authService.VerifyAuthenticatorCode(user, request.UserForLoginDto.AuthenticatorCode);
                }
            }

            AccessToken createdAccessToken = await _authService.CreateAccessToken(user!);

            await _authService.DeleteOldActiveRefreshTokens(user!);
            RefreshToken refreshToken = await _authService.CreateRefreshToken(user!, request.IpAddress);
            await _authService.AddRefreshToken(refreshToken);

            response.AccessToken = createdAccessToken;
            response.RefreshToken = refreshToken;

            return response;
        }
    }
}
