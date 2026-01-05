using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.ChangeReservationStatus;

public sealed class ChangeStaffReservationStatusCommandValidator
    : AbstractValidator<ChangeStaffReservationStatusCommand>
{
    public ChangeStaffReservationStatusCommandValidator()
    {
        RuleFor(x => x.ReservationId).NotEmpty();
        // Ensure Data is present first, then validate its properties.
        RuleFor(x => x.Data)
            .NotNull()
            .DependentRules(() =>
            {
                // Safe to access Data here because NotNull passed.
                RuleFor(x => x.Data!.Status).IsInEnum();
            });
    }
}
