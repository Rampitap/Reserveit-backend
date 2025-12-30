using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Reserveit.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPresentation(this WebApplicationBuilder builder) 
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // підстав урлу фронта
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); 
                });
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Reserveit.Auth";
                options.Cookie.HttpOnly = true;  // JS don't have access
                options.Cookie.SameSite = SameSiteMode.Lax; // local host 
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;

                // wgo redirektit na login page, my budem povertat 401, 403 status codes
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Reserveit API",
                    Version = "v1",
                    Description = "Auth via HttpOnly Cookies"
                });
            });

            /*builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    []
                }
            });
            });*/
            builder.Services.AddEndpointsApiExplorer();
        }
    }
}
