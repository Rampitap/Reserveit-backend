namespace Reserveit.Application.Extensions;

public static class TimeZoneHelper
{
    public static TimeZoneInfo Resolve(string? timezone)
    {
        if (string.IsNullOrWhiteSpace(timezone))
            return TimeZoneInfo.Utc;

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timezone);
        }
        catch
        {
            return TimeZoneInfo.Utc;
        }
    }

    public static DateTimeOffset LocalToUtc(DateTime localInTz, TimeZoneInfo tz)
    {
        var unspecified = DateTime.SpecifyKind(localInTz, DateTimeKind.Unspecified);
        var utc = TimeZoneInfo.ConvertTimeToUtc(unspecified, tz);
        return new DateTimeOffset(utc, TimeSpan.Zero);
    }

    public static DateTimeOffset LocalToOffset(DateTime localInTz, TimeZoneInfo tz)
    {
        var unspecified = DateTime.SpecifyKind(localInTz, DateTimeKind.Unspecified);
        var offset = tz.GetUtcOffset(unspecified);
        return new DateTimeOffset(unspecified, offset);
    }
}
