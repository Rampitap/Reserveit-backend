using Reserveit.Domain.Enums;

namespace Reserveit.Application.Common.DTOs.ReservationsDtos;

public sealed class ChangeReservationStatusDto
{
    public ReservationStatus Status { get; init; }
}
