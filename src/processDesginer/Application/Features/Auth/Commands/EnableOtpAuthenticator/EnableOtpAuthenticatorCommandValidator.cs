using FluentValidation;

namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommandValidator : AbstractValidator<EnableOtpAuthenticatorCommand>
{
    public EnableOtpAuthenticatorCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}