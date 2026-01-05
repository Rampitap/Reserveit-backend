using Reserveit.Domain.Exceptions;
//using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentValidation;

namespace Reserveit.API.Middlewares;

public class ErrorHadlingMiddleware(ILogger<ErrorHadlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteJson(context, new
            {
                message = "Validation failed",
                errors = ex.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                })
            });
        }
        catch (ForbiddenException ex)
        {
            logger.LogWarning(ex, "Forbidden");

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await WriteJson(context, new
            {
                message = ex.Message
            });
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "Not found");

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await WriteJson(context, new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Bad request");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteJson(context, new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteJson(context, new
            {
                message = "Internal server error"
            });
        }
    }

    private static async Task WriteJson(HttpContext context, object body)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })
        );
    }
}
