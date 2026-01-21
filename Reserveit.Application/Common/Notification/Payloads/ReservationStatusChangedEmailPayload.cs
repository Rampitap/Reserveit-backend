using Reserveit.Domain.Constants;

namespace Reserveit.Application.Common.Notification.Payloads;

public sealed class ReservationStatusChangedEmailPayload : EmailPayloadBase
{
    public Guid ReservationId { get; init; }
    public string ToRole { get; init; } = null!;
    public string OldStatus { get; init; } = null!;
    public string NewStatus { get; init; } = null!;

    public ReservationStatusChangedEmailPayload()
    {
        Type = NotificationType.ReservationStatusChanged;
    }
}