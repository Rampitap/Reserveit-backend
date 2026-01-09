using Reserveit.Domain.Entities;

namespace Reserveit.Domain.Interfaces;

public interface IStaffRepository
{
    Task<Staff?> GetByUserIdAsync(Guid userId, CancellationToken ct);

    Task<Staff?> GetByIdAsync(Guid staffId, CancellationToken ct);

    Task<List<Staff>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct);

    Task<Staff?> GetByBusinessAndIdAsync(Guid businessId, Guid staffId, CancellationToken ct);

    Task<Staff?> GetTrackedByBusinessAndIdAsync(Guid businessId, Guid staffId, CancellationToken ct);

    Task DeleteAsync(Staff staff, CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);
}
