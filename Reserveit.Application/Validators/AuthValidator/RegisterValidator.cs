using FluentValidation;
using Reserveit.Application.Common.DTOs.AuthDtod;
using Reserveit.Domain.Constants;

namespace Reserveit.Application.Validators.AuthValidator;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");

        RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")

                
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")

                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")

                
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")

                
                .Matches("[0-9]").WithMessage("Password must contain at least one digit");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");

        RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => role == UserRoles.Client || role == UserRoles.Owner)
                .WithMessage($"You can register only as '{UserRoles.Client}' or '{UserRoles.Owner}'.");
    }
}
