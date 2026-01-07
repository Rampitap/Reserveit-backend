using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.CreateStaffByOwner;

public sealed class CreateStaffCommandValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffCommandValidator()
    {
        RuleFor(x => x.Data).NotNull();

        When(x => x.Data != null, () =>
        {
            RuleFor(x => x.Data.BusinessId)
                .NotEmpty();

            RuleFor(x => x.Data.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.Data.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.Data.DisplayName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(80);

            RuleFor(x => x.Data.FirstName)
                .MaximumLength(80);

            RuleFor(x => x.Data.LastName)
                .MaximumLength(80);

            RuleFor(x => x.Data.Bio)
                .MaximumLength(500);
        });
    }
}
