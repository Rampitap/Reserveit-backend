using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Reservations.Queries.GetBusinessReservations;

public sealed class GetBusinessReservationsQueryValidator : AbstractValidator<GetBusinessReservationsQuery>
{
    public GetBusinessReservationsQueryValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.From).LessThan(x => x.To);

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
