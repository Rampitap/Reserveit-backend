namespace Reserveit.Domain.Constants;

public sealed class EmailOptions
{
    public int ReminderMinutesBefore { get; set; } = 120;
    public int SendIntervalSeconds { get; set; } = 10;
    public int BatchSize { get; set; } = 25;
}
