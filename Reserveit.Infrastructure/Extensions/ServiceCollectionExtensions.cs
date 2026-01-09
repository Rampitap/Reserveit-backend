using FluentEmail.MailKitSmtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Interfaces;
using Reserveit.Infrastructure.BackgroundServices;
using Reserveit.Infrastructure.Persistence;
using Reserveit.Infrastructure.Repositories;
using Reserveit.Infrastructure.Seeders;
using Reserveit.Infrastructure.Services;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IBusinessRepository, BusinessRepository>();
        services.AddScoped<IStaffAccountService, StaffAccountService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<INotificationQueue, NotificationQueue>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();


        #region Fluent Email Configuration
        var section = configuration.GetSection("Email");
        var smtp = section.GetSection("Smtp");

        services
            .AddFluentEmail(section["From"], section["FromName"])
            .AddMailKitSender(new SmtpClientOptions
            {
                Server = smtp["Host"]!,
                Port = int.Parse(smtp["Port"]!),
                User = smtp["User"],
                Password = smtp["Password"],
                RequiresAuthentication = !string.IsNullOrWhiteSpace(smtp["User"]),
                UseSsl = bool.Parse(smtp["UseSsl"] ?? "false")
            });

        services.AddHostedService<EmailNotificationWorker>();
        services.AddHostedService<ReservationReminderWorker>();

        #endregion  
    }
}
