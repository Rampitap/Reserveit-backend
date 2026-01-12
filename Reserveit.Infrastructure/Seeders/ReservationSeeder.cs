using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

      
        if (!dbContext.Roles.Any())
        {
            await SeedRolesAsync();
        }

        
        if (!dbContext.Users.Any(u => u.Email == "admin@reserveit.com"))
        {
            await SeedAdminAsync();
        }

        
        if (!dbContext.Businesses.Any())
        {
            await SeedFullDemoDataAsync();
        }

        if (!dbContext.Categories.Any())
        {
            await SeedCategoriesAsync();
        }
    }

    private async Task SeedFullDemoDataAsync()
    {
        
        var clientUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = "client@demo.com",
            Email = "client@demo.com",
           // FullName = "Alex Customer",
            FirstName = "Alex",
            LastName = "Customerowski",
            EmailConfirmed = true,
            
        };
        await userManager.CreateAsync(clientUser, "Client123$");
        await userManager.AddToRoleAsync(clientUser, UserRoles.Client);


       
        var barberOwner = new User
        {
            Id = Guid.NewGuid(),
            UserName = "barber@demo.com",
            Email = "barber@demo.com",
            //FullName = "John Barber Owner",
            FirstName = "John",
            LastName = "Barberownerich",
            EmailConfirmed = true,
            
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
            //FullName = "Dmytro Master",
            FirstName = "Dmytro",
            LastName = "Masterow",
            EmailConfirmed = true,
            
            
        };
        await userManager.CreateAsync(staffDmytroUser, "Staff123$");
        await userManager.AddToRoleAsync(staffDmytroUser, UserRoles.Staff);

        
        var staffDmytro = new Staff
        {
            DisplayName = "Dmytro TopMaster",
            Bio = "Senior barber with 5 years exp.",
            UserId = staffDmytroUser.Id, 
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

        var barberCategory = await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == "Barber");

        if (barberCategory == null)
            throw new Exception("Category 'Barber' not found. SeedCategoriesAsync must run first.");


        var barbershop = new Business
        {
            Name = "Elite Barbershop",
            Address = "Kyiv, Maidan 1",
            OwnerId = barberOwner.Id,
            OpeningTime = new TimeSpan(10, 0, 0),
            ClosingTime = new TimeSpan(21, 0, 0),
            IsActive = true,
            CategoryId = barberCategory.Id,
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
            //FullName = "Anna Spa",
            FirstName = "Ans",
            LastName = "Spaowner",
            EmailConfirmed = true,
            
        };
        await userManager.CreateAsync(spaOwner, "Owner123$");
        await userManager.AddToRoleAsync(spaOwner, UserRoles.Owner);

        var serviceMassage = new Service { Name = "Thai Massage", DurationMinutes = 60, Price = 1200 };

        var staffOlena = new Staff
        {
            DisplayName = "Olena",
            Services = new List<Service> { serviceMassage }
        };

        var beautyCategory = await dbContext.Categories.FirstAsync(c => c.Name == "Beauty");

        var spa = new Business
        {
            Name = "Lotus Spa",
            Address = "Lviv, Rynok 10",
            OwnerId = spaOwner.Id,
            OpeningTime = new TimeSpan(9, 0, 0),
            ClosingTime = new TimeSpan(20, 0, 0),
            CategoryId = beautyCategory.Id,
            IsActive = true,
            Services = new List<Service> { serviceMassage },
            StaffMembers = new List<Staff> { staffOlena }
        };

        dbContext.Businesses.Add(spa);

        
        await dbContext.SaveChangesAsync();

        
        staffDmytroUser.BusinessId = barbershop.Id;
        await userManager.UpdateAsync(staffDmytroUser);
    }

    
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
            //FullName = "Super Admin",
            FirstName = "Super",
            LastName = "Admin",
            EmailConfirmed = true,
            
            IsActive = true
        };
        await userManager.CreateAsync(admin, "Admin123$");
        await userManager.AddToRoleAsync(admin, UserRoles.Admin);
    }
    private async Task SeedCategoriesAsync()
    {
        if (dbContext.Categories.Any()) return;

        dbContext.Categories.AddRange(
            new Category { Id = Guid.NewGuid(), Name = "Beauty" },
            new Category { Id = Guid.NewGuid(), Name = "Barber" },
            new Category { Id = Guid.NewGuid(), Name = "Massage" },
            new Category { Id = Guid.NewGuid(), Name = "Dentistry" }
        );

        await dbContext.SaveChangesAsync();
    }
}
