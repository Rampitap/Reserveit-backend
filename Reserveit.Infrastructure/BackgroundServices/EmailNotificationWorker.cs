using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reserveit.Domain.Constants;
using Reserveit.Infrastructure.Persistence;
using System.Text.Json;

namespace Reserveit.Infrastructure.BackgroundServices;

public sealed class EmailNotificationWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<EmailNotificationWorker> _log;
    private readonly EmailOptions _opt;

    public EmailNotificationWorker(IServiceProvider sp, ILogger<EmailNotificationWorker> log, IOptions<EmailOptions> opt)
    {
        _sp = sp;
        _log = log;
        _opt = opt.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var email = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

                var batch = await db.Notifications
                    .Where(n => n.Channel == NotificationChannel.Email && n.SentAt == null)
                    .OrderBy(n => n.CreatedAt)
                    .Take(_opt.BatchSize)
                    .ToListAsync(stoppingToken);

                foreach (var n in batch)
                {
                    var to = await db.Users
                        .AsNoTracking()
                        .Where(u => u.Id == n.UserId)
                        .Select(u => u.Email)
                        .FirstOrDefaultAsync(stoppingToken);

                    if (string.IsNullOrWhiteSpace(to))
                    {
                        _log.LogWarning("Notification {Id} has empty payload", n.Id);
                        n.SentAt = DateTimeOffset.UtcNow;
                        continue;
                    }

                    var (subject, html) = await BuildEmailAsync(db, n.PayloadJson, stoppingToken);

                    var res = await email
                        .To(to)
                        .Subject(subject)
                        .Body(html, isHtml: true)
                        .SendAsync(stoppingToken);

                    if (res.Successful)
                        n.SentAt = DateTimeOffset.UtcNow;
                    else
                        _log.LogWarning("Email send failed: {Errors}", string.Join("; ", res.ErrorMessages));
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "EmailNotificationWorker error");
            }

            await Task.Delay(TimeSpan.FromSeconds(_opt.SendIntervalSeconds), stoppingToken);
        }
    }

    private static async Task<(string subject, string html)> BuildEmailAsync(AppDbContext db, string payloadJson, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(payloadJson);
        var type = doc.RootElement.GetProperty("Type").GetString();

        var reservationId = doc.RootElement.GetProperty("ReservationId").GetGuid();

        var r = await db.Reservations
            .AsNoTracking()
            .Include(x => x.Business)
            .Include(x => x.Service)
            .Include(x => x.Client)
            .Include(x => x.Staff)
            .FirstAsync(x => x.Id == reservationId, ct);

        if (type == NotificationType.ReservationCreated)
        {
            return (
                subject: $"New reservation: {r.Business.Name}",
                html: $"<h3>Reservation created</h3>" +
                      $"<p>Business: {r.Business.Name}</p>" +
                      $"<p>Service: {r.Service.Name}</p>" +
                      $"<p>Time: {r.StartAt:yyyy-MM-dd HH:mm}</p>" +
                      $"<p>Status: {r.Status}</p>"
            );
        }

        if (type == NotificationType.ReservationStatusChanged)
        {
            var oldStatus = doc.RootElement.GetProperty("OldStatus").GetString();
            var newStatus = doc.RootElement.GetProperty("NewStatus").GetString();

            return (
                subject: $"Status update: {r.Business.Name}",
                html: $"<h3>Reservation status has been changed</h3>" +
                      $"<p>{oldStatus} → <b>{newStatus}</b></p>" +
                      $"<p>Business: {r.Business.Name}</p>" +
                      $"<p>Service: {r.Service.Name}</p>" +
                      $"<p>Time: {r.StartAt:yyyy-MM-dd HH:mm}</p>"
            );
        }

        if (type == NotificationType.ReservationReminder)
        {
            return (
                subject: $"Apointment reminder: {r.Business.Name}",
                html: $"<h3>Reminder</h3>" +
                      $"<p>You have reservation om: <b>{r.StartAt:yyyy-MM-dd HH:mm}</b></p>" +
                      $"<p>Business: {r.Business.Name}</p>" +
                      $"<p>Service: {r.Service.Name}</p>"
            );
        }

        return ("Notification", "<p>Unknown notification</p>");
    }
}
