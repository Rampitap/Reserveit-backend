using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Reserveit.API.Extensions;

public static class StartupLoggingExtensions
{
    public static void LogStartupBanner(this WebApplication app)
    {
        var cfg = app.Configuration.GetSection("StartupBanner");
        if (!cfg.GetValue("Enabled", true))
            return;

        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var message = cfg.GetValue<string>("Message") ?? "API started";
            var showSwagger = cfg.GetValue("ShowSwagger", true);
            var showDb = cfg.GetValue("ShowDb", true);

            var server = app.Services.GetService<IServer>();
            var addresses = server?.Features.Get<IServerAddressesFeature>()?.Addresses?.ToArray()
                            ?? (app.Urls?.ToArray() ?? Array.Empty<string>());

            var baseUrl = addresses.FirstOrDefault() ?? "(unknown)";

            app.Logger.LogInformation("==============================================");
            app.Logger.LogInformation("{Message}", message);
            app.Logger.LogInformation("Environment: {Env}", app.Environment.EnvironmentName);
            app.Logger.LogInformation("Listening on: {Urls}", string.Join(", ", addresses));

            if (showSwagger && app.Environment.IsDevelopment())
                app.Logger.LogInformation("Swagger: {SwaggerUrl}", $"{baseUrl}/swagger");

            if (showDb)
            {
                var cs = app.Configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrWhiteSpace(cs))
                    app.Logger.LogInformation("DB: {DbInfo}", MaskConnectionString(cs));
            }

            app.Logger.LogInformation("==============================================");
        });
    }

    private static string MaskConnectionString(string cs)
    {
        var parts = cs.Split(';', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length; i++)
        {
            var p = parts[i].Trim();
            if (p.StartsWith("Password=", StringComparison.OrdinalIgnoreCase) ||
                p.StartsWith("Pwd=", StringComparison.OrdinalIgnoreCase))
            {
                var key = p.Split('=')[0];
                parts[i] = $"{key}=***";
            }
        }
        return string.Join(';', parts) + ";";
    }
}
