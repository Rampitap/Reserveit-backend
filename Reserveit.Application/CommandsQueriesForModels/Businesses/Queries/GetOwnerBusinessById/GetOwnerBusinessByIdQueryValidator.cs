using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinessById;

public sealed class GetOwnerBusinessByIdQueryValidator : AbstractValidator<GetOwnerBusinessByIdQuery>
{
    public GetOwnerBusinessByIdQueryValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
    }
}
