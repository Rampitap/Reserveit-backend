using Reserveit.Domain.Constants;

namespace Reserveit.Application.Common.Notification.Payloads;

public sealed class ReservationCreatedEmailPayload : EmailPayloadBase
{
    public Guid ReservationId { get; init; }
    public string ToRole { get; init; } = null!; // "Client" / "Staff"

    public ReservationCreatedEmailPayload()
    {
        Type = NotificationType.ReservationCreated;
    }
}