using Microsoft.AspNetCore.Identity;
using Reserveit.Domain.Constants;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Infrastructure.Persistence;
using Reserveit.Infrastructure.Seeders;
using System.Data;

internal class ReservationSeeder(
    AppDbContext dbContext,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager) : IReservationSeeder
{
    public async Task Seed()
    {
        if (!await dbContext.Database.CanConnectAsync()) return;

        // 1. Ролі
        if (!dbContext.Roles.Any())
        {
            await SeedRolesAsync();
        }

        // 2. Супер-Адмін
        if (!dbContext.Users.Any(u => u.Email == "admin@reserveit.com"))
        {
            await SeedAdminAsync();
        }

        // 3. Повні демо-дані (Бізнес + Персонал + Записи)
        if (!dbContext.Businesses.Any())
        {
            await SeedFullDemoDataAsync();
        }
    }

    private async Task SeedFullDemoDataAsync()
    {
        // --- КРОК 1: Створюємо КЛІЄНТА ---
        var clientUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = "client@demo.com",
            Email = "client@demo.com",
            FullName = "Alex Customer",
            EmailConfirmed = true,
            Role = Role.Client
        };
        await userManager.CreateAsync(clientUser, "Client123$");
        await userManager.AddToRoleAsync(clientUser, UserRoles.Client);


       
        var barberOwner = new User
        {
            Id = Guid.NewGuid(),
            UserName = "barber@demo.com",
            Email = "barber@demo.com",
            FullName = "John Barber Owner",
            EmailConfirmed = true,
            Role = Role.Owner
        };
        await userManager.CreateAsync(barberOwner, "Owner123$");
        await userManager.AddToRoleAsync(barberOwner, UserRoles.Owner);

        
        var serviceHaircut = new Service
        {
            Name = "Men's Haircut",
            Description = "Fade and style.",
            DurationMinutes = 45,
            Price = 500
        };
        var serviceBeard = new Service
        {
            Name = "Beard Trim",
            DurationMinutes = 30,
            Price = 300
        };

        var staffDmytroUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = "dmytro@staff.com",
            Email = "dmytro@staff.com",
            FullName = "Dmytro Master",
            EmailConfirmed = true,
            Role = Role.Staff,
            
        };
        await userManager.CreateAsync(staffDmytroUser, "Staff123$");
        await userManager.AddToRoleAsync(staffDmytroUser, UserRoles.Staff);

        
        var staffDmytro = new Staff
        {
            DisplayName = "Dmytro TopMaster",
            Bio = "Senior barber with 5 years exp.",
            UserId = staffDmytroUser.Id, // Зв'язок з акаунтом
            IsActive = true,
            
            Services = new List<Service> { serviceHaircut, serviceBeard }
        };

       
        var staffIvan = new Staff
        {
            DisplayName = "Ivan Intern",
            Bio = "Junior barber.",
            IsActive = true,
            Services = new List<Service> { serviceBeard }
        };

        
        var barbershop = new Business
        {
            Name = "Elite Barbershop",
            Address = "Kyiv, Maidan 1",
            OwnerId = barberOwner.Id,
            OpeningTime = new TimeSpan(10, 0, 0),
            ClosingTime = new TimeSpan(21, 0, 0),
            IsActive = true,
            Services = new List<Service> { serviceHaircut, serviceBeard },
            StaffMembers = new List<Staff> { staffDmytro, staffIvan }
        };

        
        dbContext.Businesses.Add(barbershop);

       
        var reservation = new Reservation
        {
            Business = barbershop,
            Service = serviceHaircut,
            Staff = staffDmytro,
            ClientId = clientUser.Id,
            StartAt = DateTimeOffset.UtcNow.AddDays(1).Date.AddHours(14), 
            EndAt = DateTimeOffset.UtcNow.AddDays(1).Date.AddHours(14).AddMinutes(45),
            Status = ReservationStatus.Confirmed,
            Notes = "First visit from Seeder"
        };

        dbContext.Reservations.Add(reservation);


        
        var spaOwner = new User
        {
            Id = Guid.NewGuid(),
            UserName = "spa@demo.com",
            Email = "spa@demo.com",
            FullName = "Anna Spa",
            EmailConfirmed = true,
            Role = Role.Owner
        };
        await userManager.CreateAsync(spaOwner, "Owner123$");
        await userManager.AddToRoleAsync(spaOwner, UserRoles.Owner);

        var serviceMassage = new Service { Name = "Thai Massage", DurationMinutes = 60, Price = 1200 };

        var staffOlena = new Staff
        {
            DisplayName = "Olena",
            Services = new List<Service> { serviceMassage }
        };

        var spa = new Business
        {
            Name = "Lotus Spa",
            Address = "Lviv, Rynok 10",
            OwnerId = spaOwner.Id,
            OpeningTime = new TimeSpan(9, 0, 0),
            ClosingTime = new TimeSpan(20, 0, 0),
            Services = new List<Service> { serviceMassage },
            StaffMembers = new List<Staff> { staffOlena }
        };

        dbContext.Businesses.Add(spa);

        // Зберігаємо все разом!
        await dbContext.SaveChangesAsync();

        // Оновлюємо Staff User-а, щоб прописати йому BusinessId (тепер, коли бізнес створений)
        staffDmytroUser.BusinessId = barbershop.Id;
        await userManager.UpdateAsync(staffDmytroUser);
    }

    // --- Допоміжні методи (Roles & Admin) ---
    private async Task SeedRolesAsync()
    {
        string[] roles = { UserRoles.Admin, UserRoles.Owner, UserRoles.Staff, UserRoles.Client };
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }

    private async Task SeedAdminAsync()
    {
        var admin = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin@reserveit.com",
            Email = "admin@reserveit.com",
            FullName = "Super Admin",
            EmailConfirmed = true,
            Role = Role.Admin,
            IsActive = true
        };
        await userManager.CreateAsync(admin, "Admin123$");
        await userManager.AddToRoleAsync(admin, UserRoles.Admin);
    }
}
