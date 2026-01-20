using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaff;

public sealed class UpdateOwnerStaffCommandValidator : AbstractValidator<UpdateOwnerStaffCommand>
{
    public UpdateOwnerStaffCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();

        RuleFor(x => x.Data).NotNull();

        RuleFor(x => x.Data.DisplayName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Data.Bio)
            .MaximumLength(2000);
    }
}
