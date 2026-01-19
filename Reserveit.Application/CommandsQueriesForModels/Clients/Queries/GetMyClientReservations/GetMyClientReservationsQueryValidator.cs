using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Queries.GetMyClientReservations;

public sealed class GetMyClientReservationsQueryValidator : AbstractValidator<GetMyClientReservationsQuery>
{
    public GetMyClientReservationsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue);
    }
}
