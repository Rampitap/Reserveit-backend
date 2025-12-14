using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reserveit.Domain.Entities;
using Reserveit.Infrastructure.Persistence;
using Reserveit.Infrastructure.Seeders;

namespace Reserveit.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrasrtucture(this IServiceCollection services, IConfiguration configuration) 
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

       
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentity<User, IdentityRole<Guid>>(options => { })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IReservationSeeder, ReservationSeeder>();
    }
}
