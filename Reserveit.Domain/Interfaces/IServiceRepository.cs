using Reserveit.Domain.Entities;

namespace Reserveit.Domain.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct);
}
