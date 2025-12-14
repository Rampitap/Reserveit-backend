namespace Reserveit.Domain.Entities;

public class Staff
{
    public Guid Id { get; set; }

    
    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

   
    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public string DisplayName { get; set; } = null!; 
    public string? Bio { get; set; } 

    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    // --- ЗВ'ЯЗКИ ---
    // Які послуги надає цей майстер (Many-to-Many з Service)
    public ICollection<Service> Services { get; set; } = new List<Service>();

    // Записи до цього майстра
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
