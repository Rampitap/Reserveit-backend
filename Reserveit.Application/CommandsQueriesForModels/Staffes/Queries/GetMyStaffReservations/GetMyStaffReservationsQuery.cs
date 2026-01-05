using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Domain.Enums;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetMyStaffReservations;

public sealed record GetMyStaffReservationsQuery : IRequest<IReadOnlyList<ReservationDto>>
{
    public DateTimeOffset From { get; init; }
    public DateTimeOffset To { get; init; }

    public ReservationStatus? Status { get; init; }
    public ReservationSort Sort { get; init; } = ReservationSort.StartAsc;
}
