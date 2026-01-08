using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.DeactivateOwnerService;

public sealed class DeactivateOwnerServiceCommandValidator : AbstractValidator<DeactivateOwnerServiceCommand>
{
    public DeactivateOwnerServiceCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
    }
}
