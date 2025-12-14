namespace Reserveit.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; } 
    public decimal? Price { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    
    public ICollection<Staff> Staffs { get; set; } = new List<Staff>();

    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
