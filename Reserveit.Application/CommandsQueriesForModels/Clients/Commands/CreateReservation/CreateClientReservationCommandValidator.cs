using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CreateReservation;

public sealed class CreateClientReservationCommandValidator : AbstractValidator<CreateClientReservationCommand>
{
    public CreateClientReservationCommandValidator()
    {
        RuleFor(x => x.Data).NotNull();

        RuleFor(x => x.Data.BusinessId).NotEmpty();
        RuleFor(x => x.Data.ServiceId).NotEmpty();
        RuleFor(x => x.Data.StaffId).NotEmpty();

        RuleFor(x => x.Data.StartAt)
            .NotEmpty()
            .Must(x => x > DateTimeOffset.UtcNow.AddMinutes(-1))
            .WithMessage("StartAt should be in future.");
    }
}

