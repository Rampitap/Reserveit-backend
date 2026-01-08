using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaffStatus;

public sealed class UpdateOwnerStaffStatusCommandValidator : AbstractValidator<UpdateOwnerStaffStatusCommand>
{
    public UpdateOwnerStaffStatusCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.Data).NotNull();
    }
}
