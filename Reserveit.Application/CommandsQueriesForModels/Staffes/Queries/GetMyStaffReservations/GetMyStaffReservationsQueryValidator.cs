using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetMyStaffReservations;

public sealed class GetMyStaffReservationsQueryValidator
    : AbstractValidator<GetMyStaffReservationsQuery>
{
    public GetMyStaffReservationsQueryValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();

        RuleFor(x => x)
            .Must(x => x.From < x.To)
            .WithMessage("'From' has to be earlier than 'To'.");

        RuleFor(x => x)
            .Must(x => (x.To - x.From).TotalDays <= 62)
            .WithMessage("Range is too wide, 62 days at max");

        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
