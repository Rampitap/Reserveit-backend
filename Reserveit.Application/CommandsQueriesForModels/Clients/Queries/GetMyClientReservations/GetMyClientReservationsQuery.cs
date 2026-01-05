using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Queries.GetMyClientReservations;

public sealed record GetMyClientReservationsQuery(int Page = 1, int PageSize = 10)
    : IRequest<IReadOnlyList<ReservationDto>>;