using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Reservations.Commands.ChangeBusinessReservationStatus;

public sealed class ChangeBusinessReservationStatusCommandValidator
    : AbstractValidator<ChangeBusinessReservationStatusCommand>
{
    public ChangeBusinessReservationStatusCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.ReservationId).NotEmpty();
        RuleFor(x => x.Data).NotNull();
        RuleFor(x => x.Data!.Status).IsInEnum();
    }
}
