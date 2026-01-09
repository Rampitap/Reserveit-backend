using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.DeleteOwnerBusiness;

public sealed class DeleteOwnerBusinessCommandValidator : AbstractValidator<DeleteOwnerBusinessCommand>
{
    public DeleteOwnerBusinessCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
    }
}
