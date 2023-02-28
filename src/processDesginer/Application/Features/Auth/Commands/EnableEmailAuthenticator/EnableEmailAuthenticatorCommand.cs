using Application.Features.Auth.Constans;
using Application.Features.Auth.Rules;
using Application.Services.Core.AuthService;
using Application.Services.Core.Repositories;
using Crea.Core.Application.Pipelines.Authorization;
using Crea.Core.Mailing;
using Crea.Core.Security.Entities;
using MediatR;
using System.Web;

namespace Application.Features.Auth.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand : IRequest, ISecuredOperation
{
    public int UserId { get; set; }
    public string VerifyEmailUrl { get; set; }
    public string[] Roles => Array.Empty<string>();

    public class EnableEmailAuthenticatorCommandHandler : IRequestHandler<EnableEmailAuthenticatorCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUserEmailAuthenticatorRepository _userEmailAuthenticatorRepository;
        private readonly IMailService _mailService;
        private AuthBusinessRules _authBusinessRules;

        public EnableEmailAuthenticatorCommandHandler(IUserRepository userRepository, IAuthService authService, IUserEmailAuthenticatorRepository userEmailAuthenticatorRepository, IMailService mailService, AuthBusinessRules authBusinessRules)
        {
            _userRepository = userRepository;
            _authService = authService;
            _userEmailAuthenticatorRepository = userEmailAuthenticatorRepository;
            _mailService = mailService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<Unit> Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(x => x.Id == request.UserId);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserShouldNotHasAuthenticator(user!);

            await _userEmailAuthenticatorRepository.DeleteAllNonVerifiedAsync(user!);
            UserEmailAuthenticator userEmailAuthenticator = await _authService.CreateEmailAuthenticator(user!);
            await _userEmailAuthenticatorRepository.AddAsync(userEmailAuthenticator);

            Mail mailData = new()
            {
                ToEmail = user!.Email,
                ToFullName = $"{user.FirstName} {user.LastName}",
                Subject = AuthBusinessMessages.VerifyEmail,
                TextBody = $"{AuthBusinessMessages.ClickOnBelowVerifyEmail}\n\n{request.VerifyEmailUrl}?activationKey={HttpUtility.UrlEncode(userEmailAuthenticator.Key)}"
            };

            await _mailService.SendAsync(mailData);

            return Unit.Value;
        }
    }
}
