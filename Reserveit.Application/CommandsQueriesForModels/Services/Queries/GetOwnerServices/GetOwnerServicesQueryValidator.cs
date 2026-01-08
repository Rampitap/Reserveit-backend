using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Queries.GetOwnerServices;

public sealed class GetOwnerServicesQueryValidator : AbstractValidator<GetOwnerServicesQuery>
{
    public GetOwnerServicesQueryValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
    }
}