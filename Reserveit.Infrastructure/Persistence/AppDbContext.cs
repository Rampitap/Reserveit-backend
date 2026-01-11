using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;

namespace Reserveit.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Auditlog> AuditLogs { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 

        //Business -> Owner
        builder.Entity<Business>()
            .HasOne(b => b.Owner)
            .WithMany()
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        //User (Staff) -> Business
        builder.Entity<User>()
            .HasOne(u => u.WorksAtBusiness)
            .WithMany()
            .HasForeignKey(u => u.BusinessId)
            .OnDelete(DeleteBehavior.SetNull);

        //Staff -> User account
        builder.Entity<Staff>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        //Many-to-Many: Staff <-> Service
        builder.Entity<Staff>()
            .HasMany(s => s.Services)
            .WithMany(s => s.Staffs)
            .UsingEntity(j => j.ToTable("StaffServices"));

        //Reservation -> User
        builder.Entity<Reservation>()
            .HasOne(r => r.Client)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        
        builder.Entity<Business>().Property(b => b.OpeningTime).HasColumnType("time");
        builder.Entity<Business>().Property(b => b.ClosingTime).HasColumnType("time");

        builder.Entity<Category>()
             .Property(c => c.Name)
             .HasMaxLength(100)
             .IsRequired();

        builder.Entity<Business>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Businesses)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
