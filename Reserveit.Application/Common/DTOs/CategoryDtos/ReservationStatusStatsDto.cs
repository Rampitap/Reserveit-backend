namespace Reserveit.Application.Common.DTOs.CategoryDtos;

public sealed class ReservationStatusStatsDto
{
    public int Pending { get; init; }
    public int Confirmed { get; init; }
    public int Cancelled { get; init; }
    public int Completed { get; init; }
}
