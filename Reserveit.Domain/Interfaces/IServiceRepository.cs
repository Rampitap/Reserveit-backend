using Reserveit.Domain.Entities;

namespace Reserveit.Domain.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Service>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct);

    Task<Service?> GetTrackedByIdAsync(Guid id, CancellationToken ct); // for update/deactivate (tracking)

    Task AddAsync(Service service, CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);

    Task AddStaffToServiceAsync(Guid businessId, Guid serviceId, Guid staffId, CancellationToken ct);
    Task RemoveStaffFromServiceAsync(Guid businessId, Guid serviceId, Guid staffId, CancellationToken ct);
}
