using Reserveit.Domain.Constants;

namespace Reserveit.Application.Common.Notification.Payloads;

public sealed class ReservationStatusChangedEmailPayload
{
    public string Type { get; init; } = NotificationType.ReservationStatusChanged;
    public Guid ReservationId { get; init; }
    public string ToRole { get; init; } = null!;
    public string OldStatus { get; init; } = null!;
    public string NewStatus { get; init; } = null!;
}
