using Reserveit.Domain.Exceptions;

namespace Reserveit.API.Middlewares;

public class ErrorHadlingMiddleware(ILogger<ErrorHadlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFound.Message);

            logger.LogInformation(notFound.Message);
        }
        catch (ForbidException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Access forbidden");
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}
