using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reserveit.Application.Common.Notification.Payloads;
using Reserveit.Domain.Constants;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Infrastructure.Persistence;
using System.Text.Json;

namespace Reserveit.Infrastructure.BackgroundServices;

public sealed class ReservationReminderWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<ReservationReminderWorker> _log;
    private readonly EmailOptions _opt;

    public ReservationReminderWorker(
        IServiceProvider sp,
        ILogger<ReservationReminderWorker> log,
        IOptions<EmailOptions> opt)
    {
        _sp = sp;
        _log = log;
        _opt = opt.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // how odten do we check for new reminders to enqueue
        var tick = TimeSpan.FromMinutes(5);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var nowUtc = DateTimeOffset.UtcNow;

                var targetFrom = nowUtc.AddMinutes(_opt.ReminderMinutesBefore);
                var targetTo = targetFrom.AddMinutes(5); // window size = tick

                // search for reservations starting in [targetFrom..targetTo)
                var candidates = await db.Reservations
                    .AsNoTracking()
                    .Where(r =>
                        (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed) &&
                        r.StartAt >= targetFrom &&
                        r.StartAt < targetTo)
                    .Select(r => new { r.Id, r.ClientId })
                    .ToListAsync(stoppingToken);

                if (candidates.Count == 0)
                {
                    await Task.Delay(tick, stoppingToken);
                    continue;
                }

                var reservationIds = candidates.Select(x => x.Id).ToList();

                // check existing reminders
                // payloadJson contains Type + ReservationId, so we check by:
                // ReservationId + Type == Reminder
                

                var existingReminderReservationIds = await db.Notifications
                    .AsNoTracking()
                    .Where(n =>
                        n.Channel == NotificationChannel.Email &&
                        n.ReservationId != null &&
                        reservationIds.Contains(n.ReservationId.Value) &&
                        n.PayloadJson != null &&
                        n.PayloadJson.Contains(NotificationType.ReservationReminder))
                    .Select(n => n.ReservationId!.Value)
                    .Distinct()
                    .ToListAsync(stoppingToken);

                var toEnqueue = candidates
                    .Where(x => !existingReminderReservationIds.Contains(x.Id))
                    .ToList();

                if (toEnqueue.Count == 0)
                {
                    await Task.Delay(tick, stoppingToken);
                    continue;
                }

                foreach (var item in toEnqueue)
                {
                    db.Notifications.Add(new Notification
                    {
                        Id = Guid.NewGuid(),
                        UserId = item.ClientId,
                        ReservationId = item.Id,
                        Channel = NotificationChannel.Email,
                        PayloadJson = JsonSerializer.Serialize(new ReservationReminderEmailPayload
                        {
                            ReservationId = item.Id
                        }),
                        CreatedAt = DateTimeOffset.UtcNow
                    });
                }

                await db.SaveChangesAsync(stoppingToken);

                _log.LogInformation("Enqueued {Count} reminder notifications", toEnqueue.Count);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "ReservationReminderWorker error");
            }

            await Task.Delay(tick, stoppingToken);
        }
    }
}
