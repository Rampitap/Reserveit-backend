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
    public Task<Business?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Business>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct);

    Task AddAsync(Business business, CancellationToken ct);

    Task DeleteAsync(Business business, CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);

    Task<bool> HasFutureConfirmedReservationsAsync(Guid businessId, DateTimeOffset nowUtc, CancellationToken ct);
}
