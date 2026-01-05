using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.ChangeReservationStatus;

public sealed record ChangeStaffReservationStatusCommand(Guid ReservationId, ChangeReservationStatusDto Data) : IRequest;
