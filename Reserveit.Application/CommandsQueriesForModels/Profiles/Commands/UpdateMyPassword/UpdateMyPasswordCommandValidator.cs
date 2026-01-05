using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Profiles.Commands.UpdateMyPassword;

public sealed class UpdateMyPasswordCommandValidator : AbstractValidator<UpdateMyPasswordCommand>
{
    public UpdateMyPasswordCommandValidator()
    {
        RuleFor(x => x.Data.CurrentPassword).NotEmpty();
        RuleFor(x => x.Data.NewPassword)
            .NotEmpty().WithMessage("Password is required")


            .MinimumLength(8).WithMessage("Password must be at least 8 characters")

            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
    
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")

            .Matches("[0-9]").WithMessage("Password must contain at least one digit");
    }
}