using FluentValidation;
using Reserveit.Application.Common.DTOs.AuthDtod;

namespace Reserveit.Application.Validators.AuthValidator;

public class LogInValidator : AbstractValidator<LoginDto>
{
    public LogInValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
