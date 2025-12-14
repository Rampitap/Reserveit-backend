namespace Reserveit.Domain.Entities;

public class Auditlog
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = null!; 
    public Guid? EntityId { get; set; }
    public string Action { get; set; } = null!; 

    public Guid? PerformedBy { get; set; }
    public string? MetaJson { get; set; } 

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
