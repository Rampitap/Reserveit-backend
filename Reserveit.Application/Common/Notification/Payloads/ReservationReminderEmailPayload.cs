using Reserveit.Domain.Constants;

namespace Reserveit.Application.Common.Notification.Payloads;

public sealed class ReservationReminderEmailPayload : EmailPayloadBase
{
    public Guid ReservationId { get; init; }

    public ReservationReminderEmailPayload()
    {
        Type = NotificationType.ReservationReminder;
    }
}
