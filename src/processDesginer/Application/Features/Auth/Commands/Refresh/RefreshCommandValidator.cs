using FluentValidation;

namespace Application.Features.Auth.Commands.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x=>x.RefreshToken).NotEmpty();
        RuleFor(x=>x.IpAddress).NotEmpty();
    }
}
