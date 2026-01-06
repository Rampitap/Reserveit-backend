using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CancelClientReservation;

public sealed record CancelClientReservationCommand(Guid ReservationId) : IRequest;
