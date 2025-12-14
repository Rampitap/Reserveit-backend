using Microsoft.AspNetCore.Identity;
using Reserveit.Domain.Enums;

namespace Reserveit.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public Role Role { get; set; } = Role.Client;
    public string? Timezone { get; set; } 

    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    
    public Guid? BusinessId { get; set; }
    public Business? WorksAtBusiness { get; set; }

    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
