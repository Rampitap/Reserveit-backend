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
                    // 1) recipient
                    var to = await db.Users
                        .AsNoTracking()
                        .Where(u => u.Id == n.UserId)
                        .Select(u => u.Email)
                        .FirstOrDefaultAsync(stoppingToken);

                    if (string.IsNullOrWhiteSpace(to))
                    {
                        _log.LogWarning("Notification {Id} skipped: user has no email. UserId={UserId}", n.Id, n.UserId);
                        n.SentAt = DateTimeOffset.UtcNow; 
                        continue;
                    }

                    // 2) payload
                    if (string.IsNullOrWhiteSpace(n.PayloadJson))
                    {
                        _log.LogWarning("Notification {Id} skipped: empty PayloadJson.", n.Id);
                        n.SentAt = DateTimeOffset.UtcNow;
                        continue;
                    }

                    // 3) build email
                    var built = await TryBuildEmailAsync(db, n.PayloadJson, stoppingToken);
                    if (built is null)
                    {
                        _log.LogWarning("Notification {Id} skipped: invalid payload JSON. Payload={Payload}", n.Id, n.PayloadJson);
                        n.SentAt = DateTimeOffset.UtcNow;
                        continue;
                    }

                    var (subject, html) = built.Value;

                    // 4) send
                    var res = await email
                        .To(to)
                        .Subject(subject)
                        .Body(html, isHtml: true)
                        .SendAsync(stoppingToken);

                    if (res.Successful)
                    {
                        n.SentAt = DateTimeOffset.UtcNow;
                    }
                    else
                    {
                        _log.LogWarning("Email send failed. NotificationId={Id}. Errors={Errors}",
                            n.Id, string.Join("; ", res.ErrorMessages));
                        
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // graceful shutdown
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "EmailNotificationWorker error");
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(_opt.SendIntervalSeconds), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // ignore
            }
        }
    }

    private static async Task<(string subject, string html)?> TryBuildEmailAsync(AppDbContext db, string payloadJson, CancellationToken ct)
    {
        JsonDocument doc;
        try
        {
            doc = JsonDocument.Parse(payloadJson);
        }
        catch
        {
            return null;
        }

        using (doc)
        {
            var root = doc.RootElement;

            if (!TryGetString(root, "Type", out var type))
                return null;

            if (!TryGetGuid(root, "ReservationId", out var reservationId))
                return null;

            var r = await db.Reservations
                .AsNoTracking()
                .Include(x => x.Business)
                .Include(x => x.Service)
                .Include(x => x.Client)
                .Include(x => x.Staff)
                .FirstOrDefaultAsync(x => x.Id == reservationId, ct);

            if (r is null)
            {
                
                return ("Notification", "<p>Reservation not found.</p>");
            }

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
                
                TryGetString(root, "OldStatus", out var oldStatus);
                TryGetString(root, "NewStatus", out var newStatus);

                oldStatus ??= "?";
                newStatus ??= "?";

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
                    subject: $"Appointment reminder: {r.Business.Name}",
                    html: $"<h3>Reminder</h3>" +
                          $"<p>You have reservation on: <b>{r.StartAt:yyyy-MM-dd HH:mm}</b></p>" +
                          $"<p>Business: {r.Business.Name}</p>" +
                          $"<p>Service: {r.Service.Name}</p>"
                );
            }

            return ("Notification", "<p>Unknown notification type.</p>");
        }
    }

    private static bool TryGetString(JsonElement root, string name, out string? value)
    {
        value = null;
        if (!root.TryGetProperty(name, out var el))
            return false;

        if (el.ValueKind == JsonValueKind.String)
        {
            value = el.GetString();
            return !string.IsNullOrWhiteSpace(value);
        }

        return false;
    }

    private static bool TryGetGuid(JsonElement root, string name, out Guid value)
    {
        value = default;
        if (!root.TryGetProperty(name, out var el))
            return false;

        
        if (el.ValueKind == JsonValueKind.String)
            return Guid.TryParse(el.GetString(), out value);

        if (el.ValueKind == JsonValueKind.Undefined || el.ValueKind == JsonValueKind.Null)
            return false;

        try
        {
            value = el.GetGuid();
            return value != Guid.Empty;
        }
        catch
        {
            return false;
        }
    }
}
