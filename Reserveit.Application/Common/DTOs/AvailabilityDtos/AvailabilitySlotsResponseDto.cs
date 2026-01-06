namespace Reserveit.Application.Common.DTOs.AvailabilityDtos;

public sealed class AvailabilitySlotsResponseDto
{
    public Guid BusinessId { get; set; }
    public Guid StaffId { get; set; }
    public Guid ServiceId { get; set; }

    // Date in business timzone (YYYY-MM-DD)
    public string Date { get; set; } = null!;

    public string Timezone { get; set; } = "UTC";
    public int DurationMinutes { get; set; }
    public int StepMinutes { get; set; }

    // Working hours (for UI)
    public string OpeningTime { get; set; } = null!;
    public string ClosingTime { get; set; } = null!;

    // ready free start times in business timezone (with offset)
    public List<DateTimeOffset> AvailableStartTimes { get; set; } = new();
}
