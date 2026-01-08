using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.DeleteOwnerStaff;

public sealed class DeleteOwnerStaffCommandValidator : AbstractValidator<DeleteOwnerStaffCommand>
{
    public DeleteOwnerStaffCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
    }
}
