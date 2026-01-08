using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.AddStaffToService;

public sealed class AddStaffToServiceCommandValidator : AbstractValidator<AddStaffToServiceCommand>
{
    public AddStaffToServiceCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
    }
}
