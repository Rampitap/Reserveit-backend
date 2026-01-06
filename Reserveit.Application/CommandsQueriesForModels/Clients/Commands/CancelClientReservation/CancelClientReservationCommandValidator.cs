using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CancelClientReservation;

public sealed class CancelClientReservationCommandValidator : AbstractValidator<CancelClientReservationCommand>
{
    public CancelClientReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId).NotEmpty();
    }
}