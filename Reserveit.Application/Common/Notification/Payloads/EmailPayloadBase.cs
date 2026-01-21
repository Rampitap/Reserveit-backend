namespace Reserveit.Application.Common.Notification.Payloads;

public abstract class EmailPayloadBase
{
    public string Type { get; init; } = null!;
}