using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.RemoveStaffFromService;

public sealed class RemoveStaffFromServiceCommandValidator : AbstractValidator<RemoveStaffFromServiceCommand>
{
    public RemoveStaffFromServiceCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
    }
}
