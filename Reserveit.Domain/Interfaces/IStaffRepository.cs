using Reserveit.Domain.Entities;

namespace Reserveit.Domain.Interfaces;

public interface IStaffRepository
{
    Task<Staff?> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task<Staff?> GetByIdAsync(Guid staffId, CancellationToken ct);
}
