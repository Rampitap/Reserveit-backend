namespace Reserveit.Domain.Constants;

public sealed class EmailOptions
{
    public string From { get; set; } = null!;
    public string FromName { get; set; } = "Reserveit";

    public SmtpOptions Smtp { get; set; } = new ();

    public int ReminderMinutesBefore { get; set; } = 120;
    public int SendIntervalSeconds { get; set; } = 10;
    public int BatchSize { get; set; } = 25;
}
