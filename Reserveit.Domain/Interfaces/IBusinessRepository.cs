using Reserveit.Domain.Entities;

namespace Reserveit.Domain.Interfaces;

public interface IBusinessRepository
{
    Task<Business?> GetPublicByIdAsync(Guid businessId, CancellationToken ct);
    Task<List<Service>> GetPublicServicesAsync(Guid businessId, CancellationToken ct);
    Task<List<Staff>> GetPublicStaffAsync(Guid businessId, CancellationToken ct);

    Task<List<Business>> SearchPublicAsync(int page, int pageSize, string? q, CancellationToken ct);
    Task<int> CountPublicAsync(string? q, CancellationToken ct);

    Task<bool> IsOwnedByAsync(Guid businessId, Guid ownerId, CancellationToken ct);
}
