using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Reservations.Commands.ChangeBusinessReservationStatus;

public sealed record ChangeBusinessReservationStatusCommand(
    Guid BusinessId,
    Guid ReservationId,
    ChangeReservationStatusDto Data) : IRequest;
