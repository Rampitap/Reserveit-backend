using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusinessStatus;

public sealed class UpdateOwnerBusinessStatusCommandValidator : AbstractValidator<UpdateOwnerBusinessStatusCommand>
{
    public UpdateOwnerBusinessStatusCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
    }
}
