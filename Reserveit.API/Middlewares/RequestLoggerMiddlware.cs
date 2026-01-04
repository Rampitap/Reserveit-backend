using System.Diagnostics;

namespace Reserveit.API.Middlewares;

public class RequestLoggerMiddlware(ILogger<RequestLoggerMiddlware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopWatch.Stop();

        if (stopWatch.ElapsedMilliseconds / 1000 > 4)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var time = stopWatch.ElapsedMilliseconds;
            logger.LogInformation("Request {Method} at {Path} took {Time} ms", method, path, time);
        }
    }
}
