namespace Reserveit.Domain.Entities;

public class Business
{
    public Guid Id { get; set; }


    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;


    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string Timezone { get; set; } = "UTC";
    public TimeSpan? OpeningTime { get; set; }
    public TimeSpan? ClosingTime { get; set; }
    public string? CancellationPolicyJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}