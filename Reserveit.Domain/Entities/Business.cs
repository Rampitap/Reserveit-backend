namespace Reserveit.Domain.Entities;

public class Business
{
    public Guid Id { get; set; }


    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string Timezone { get; set; } = "UTC";
    public TimeSpan? OpeningTime { get; set; }
    public TimeSpan? ClosingTime { get; set; }
    public string? CancellationPolicyJson { get; set; }
    public string? ImageUrl { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; } = true;

    
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}