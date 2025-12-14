using Reserveit.Domain.Enums;

namespace Reserveit.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }

    
    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    
    public Guid ClientId { get; set; }
    public User Client { get; set; } = null!;

   
    public Guid? StaffId { get; set; }
    public Staff? Staff { get; set; }

  
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
