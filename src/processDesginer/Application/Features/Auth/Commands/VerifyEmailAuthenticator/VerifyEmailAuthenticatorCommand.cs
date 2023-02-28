using Application.Features.Auth.Rules;
using Application.Services.Core.Repositories;
using Crea.Core.Application.Pipelines.Authorization;
using Crea.Core.Security.Authenticator;
using Crea.Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.VerifyEmailAuthenticator;

public class VerifyEmailAuthenticatorCommand : IRequest, ISecuredOperation
{
    public int UserId { get; set; }
    public string ActivationKey { get; set; }
    public string[] Roles => Array.Empty<string>();

    public class VerifyEmailAuthenticatorCommandHandler : IRequestHandler<VerifyEmailAuthenticatorCommand>
    {
        private readonly IUserEmailAuthenticatorRepository _userEmailAuthenticatorRepository;
        private readonly AuthBusinessRules _authBusinessRules;

        public VerifyEmailAuthenticatorCommandHandler(IUserEmailAuthenticatorRepository userEmailAuthenticatorRepository, AuthBusinessRules authBusinessRules)
        {
            _userEmailAuthenticatorRepository = userEmailAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<Unit> Handle(VerifyEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            UserEmailAuthenticator? userEmailAuthenticator =
                await _userEmailAuthenticatorRepository
                .GetAsync(x => x.Key == request.ActivationKey &&
                               x.UserId == request.UserId,
                          include: x => x.Include(x => x.User));

            if (userEmailAuthenticator == null)
            {
                throw new Exception("Activation key is not valid");
            }

            await _authBusinessRules.UserEmailAuthenticatorShouldBeExists(userEmailAuthenticator);

            userEmailAuthenticator.IsVerified= true;
            userEmailAuthenticator.Key = null;
            userEmailAuthenticator.User.AuthenticatorType = AuthenticatorType.Email;

            await _userEmailAuthenticatorRepository.UpdateAsync(userEmailAuthenticator);

            return Unit.Value;
        }
    }
}
