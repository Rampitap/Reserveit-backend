using Reserveit.Domain.Constants;

namespace Reserveit.Application.Common.Notification.Payloads;

public sealed class ReservationReminderEmailPayload
{
    public string Type { get; init; } = NotificationType.ReservationReminder;
    public Guid ReservationId { get; init; }
}
