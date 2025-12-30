using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;
using Reserveit.Infrastructure.Persistence;
using Reserveit.Infrastructure.Seeders;
using Reserveit.Infrastructure.Services;

namespace Reserveit.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrasrtucture(this IServiceCollection services, IConfiguration configuration) 
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

       
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            
            options.Password.RequiredUniqueChars = 2;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;

            
            options.User.RequireUniqueEmail = true;
        })
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();
        services.AddAuthorizationBuilder();
        services.AddScoped<IReservationSeeder, ReservationSeeder>();
        services.AddScoped<IAuthService, AuthService>();
    }
}
