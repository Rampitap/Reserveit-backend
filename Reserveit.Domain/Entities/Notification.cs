namespace Reserveit.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; } 
    public Guid? ReservationId { get; set; } 

    public string Channel { get; set; } = "email"; 
    public string? PayloadJson { get; set; } 

    public DateTimeOffset? SentAt { get; set; } 
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
