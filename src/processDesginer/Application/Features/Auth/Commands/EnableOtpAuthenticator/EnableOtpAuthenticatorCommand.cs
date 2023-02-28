using Application.Features.Auth.Rules;
using Application.Services.Core.AuthService;
using Application.Services.Core.Repositories;
using Crea.Core.Application.Pipelines.Authorization;
using Crea.Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand:IRequest<EnabledOtpAuthenticatorResponse>, ISecuredOperation
{
    public int UserId { get; set; }
    public string SecretKeyLabel { get; set; }
    public string SecretKeyIssuer { get; set; }

    public string[] Roles => Array.Empty<string>();

    public class EnableOtpAuthenticatorCommandHandler : IRequestHandler<EnableOtpAuthenticatorCommand, EnabledOtpAuthenticatorResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserOtpAuthenticatorRepository _userOtpAuthenticatorRepository;

        public EnableOtpAuthenticatorCommandHandler(IUserRepository userRepository, IAuthService authService, AuthBusinessRules authBusinessRules, IUserOtpAuthenticatorRepository userOtpAuthenticatorRepository)
        {
            _userRepository = userRepository;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _userOtpAuthenticatorRepository = userOtpAuthenticatorRepository;
        }

        public async Task<EnabledOtpAuthenticatorResponse> Handle(EnableOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(x=>x.Id == request.UserId);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserShouldNotHasAuthenticator(user);


            UserOtpAuthenticator createdUserOtpAuthenticator=await _authService.CreateOtpAuthenticator(user);
            await _userOtpAuthenticatorRepository.AddAsync(createdUserOtpAuthenticator);
            string base32SecretKey = await _authService.ConvertOtpSecretKeyToString(createdUserOtpAuthenticator.SecretKey);

            EnabledOtpAuthenticatorResponse response = new()
            {
                SecretKey = await _authService.ConvertOtpSecretKeyToString(createdUserOtpAuthenticator.SecretKey),
                SecretKeyUrl = $"otpauth://totp/{request.SecretKeyLabel}?secret={base32SecretKey}&issuer={request.SecretKeyIssuer}"
            };
            return response;
        }
    }
}
