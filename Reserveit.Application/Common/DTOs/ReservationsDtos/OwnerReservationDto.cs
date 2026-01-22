namespace Reserveit.Application.Common.DTOs.ReservationsDtos;

public sealed class OwnerReservationDto
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }
    public string BusinessName { get; set; } = null!;
    public string? BusinessAddress { get; set; }

    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public decimal? Price { get; set; }
    public int DurationMinutes { get; set; }

    public Guid? StaffId { get; set; }
    public string? StaffName { get; set; }
    public string? StaffEmail { get; set; }

    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = null!;
    public string? ClientEmail { get; set; }

    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
}
