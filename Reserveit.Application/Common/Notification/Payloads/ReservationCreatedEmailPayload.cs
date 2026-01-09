using Reserveit.Domain.Constants;

namespace Reserveit.Application.Common.Notification.Payloads;

public sealed class ReservationCreatedEmailPayload
{
    public string Type { get; init; } = NotificationType.ReservationCreated;
    public Guid ReservationId { get; init; }
    public string ToRole { get; init; } = null!; // "Client" / "Staff"
}
