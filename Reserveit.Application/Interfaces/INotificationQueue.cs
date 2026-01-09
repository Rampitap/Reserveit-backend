namespace Reserveit.Application.Interfaces;

public interface INotificationQueue
{
    Task EnqueueReservationCreatedAsync(Guid reservationId, CancellationToken ct);
    Task EnqueueReservationStatusChangedAsync(Guid reservationId, string oldStatus, string newStatus, CancellationToken ct);
}
