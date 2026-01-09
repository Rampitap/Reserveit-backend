using Microsoft.EntityFrameworkCore;
using Reserveit.Application.Common.Notification.Payloads;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Constants;
using Reserveit.Domain.Entities;
using Reserveit.Infrastructure.Persistence;
using System.Text.Json;

namespace Reserveit.Infrastructure.Services;

public sealed class NotificationQueue : INotificationQueue
{
    private readonly AppDbContext _db;

    public NotificationQueue(AppDbContext db) => _db = db;

    public async Task EnqueueReservationCreatedAsync(Guid reservationId, CancellationToken ct)
    {
        // detection staff#client from reservation
        var r = await _db.Reservations
            .AsNoTracking()
            .Select(x => new { x.Id, x.ClientId, x.StaffId })
            .FirstOrDefaultAsync(x => x.Id == reservationId, ct);

        if (r is null) return;

        // client
        _db.Notifications.Add(new Notification
        {
            Id = Guid.NewGuid(),
            UserId = r.ClientId,
            ReservationId = r.Id,
            Channel = NotificationChannel.Email,
            PayloadJson = JsonSerializer.Serialize(new ReservationCreatedEmailPayload
            {
                ReservationId = r.Id,
                ToRole = "Client"
            }),
            CreatedAt = DateTimeOffset.UtcNow
        });

        // staff 
        if (r.StaffId.HasValue)
        {
            var staffUserId = await _db.Staffs
                .AsNoTracking()
                .Where(s => s.Id == r.StaffId.Value)
                .Select(s => s.UserId)
                .FirstOrDefaultAsync(ct);

            if (staffUserId.HasValue)
            {
                _db.Notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = staffUserId.Value,
                    ReservationId = r.Id,
                    Channel = NotificationChannel.Email,
                    PayloadJson = JsonSerializer.Serialize(new ReservationCreatedEmailPayload
                    {
                        ReservationId = r.Id,
                        ToRole = "Staff"
                    }),
                    CreatedAt = DateTimeOffset.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task EnqueueReservationStatusChangedAsync(Guid reservationId, string oldStatus, string newStatus, CancellationToken ct)
    {
        var r = await _db.Reservations
            .AsNoTracking()
            .Select(x => new { x.Id, x.ClientId, x.StaffId })
            .FirstOrDefaultAsync(x => x.Id == reservationId, ct);

        if (r is null) return;

        // client
        _db.Notifications.Add(new Notification
        {
            Id = Guid.NewGuid(),
            UserId = r.ClientId,
            ReservationId = r.Id,
            Channel = NotificationChannel.Email,
            PayloadJson = JsonSerializer.Serialize(new ReservationStatusChangedEmailPayload
            {
                ReservationId = r.Id,
                ToRole = "Client",
                OldStatus = oldStatus,
                NewStatus = newStatus
            }),
            CreatedAt = DateTimeOffset.UtcNow
        });

        // staff
        if (r.StaffId.HasValue)
        {
            var staffUserId = await _db.Staffs
                .AsNoTracking()
                .Where(s => s.Id == r.StaffId.Value)
                .Select(s => s.UserId)
                .FirstOrDefaultAsync(ct);

            if (staffUserId.HasValue)
            {
                _db.Notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = staffUserId.Value,
                    ReservationId = r.Id,
                    Channel = NotificationChannel.Email,
                    PayloadJson = JsonSerializer.Serialize(new ReservationStatusChangedEmailPayload
                    {
                        ReservationId = r.Id,
                        ToRole = "Staff",
                        OldStatus = oldStatus,
                        NewStatus = newStatus
                    }),
                    CreatedAt = DateTimeOffset.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync(ct);
    }
}
