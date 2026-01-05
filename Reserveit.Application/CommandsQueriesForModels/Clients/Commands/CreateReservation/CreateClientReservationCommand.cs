using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CreateReservation;

public sealed record CreateClientReservationCommand(CreateReservationDto Data) : IRequest<Guid>;

