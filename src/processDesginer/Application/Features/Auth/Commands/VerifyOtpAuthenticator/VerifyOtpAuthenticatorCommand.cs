using Application.Features.Auth.Rules;
using Application.Services.Core.AuthService;
using Application.Services.Core.Repositories;
using Crea.Core.Application.Pipelines.Authorization;
using Crea.Core.Security.Authenticator;
using Crea.Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.VerifyOtpAuthenticator;

public class VerifyOtpAuthenticatorCommand : IRequest, ISecuredOperation
{
    public int UserId { get; set; }
    public string OtpCode { get; set; }

    public string[] Roles => Array.Empty<string>();

    public class VerifyOtpAuthenticatorCommandHandler : IRequestHandler<VerifyOtpAuthenticatorCommand>
    {
        private readonly IUserOtpAuthenticatorRepository _userOtpAuthenticatorRepository;
        private readonly IAuthService _authService;
        private readonly AuthBusinessRules _authBusinessRules;

        public async Task<Unit> Handle(VerifyOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            UserOtpAuthenticator? userOtpAuthenticator =
                await _userOtpAuthenticatorRepository.GetAsync(
                    x => x.UserId == request.UserId,
                    include: x => x.Include(x => x.User));

            await _authBusinessRules.UserOtpAuthenticatorShouldBeExists(userOtpAuthenticator);
            userOtpAuthenticator!.User.AuthenticatorType = AuthenticatorType.Otp;
            await _authService.VerifyAuthenticatorCode(userOtpAuthenticator.User, request.OtpCode);
            userOtpAuthenticator.IsVerified= true;

            await _userOtpAuthenticatorRepository.UpdateAsync(userOtpAuthenticator);

            return Unit.Value;
        }
    }
}
