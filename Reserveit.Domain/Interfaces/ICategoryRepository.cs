using Reserveit.Domain.Entities;

namespace Reserveit.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken ct);
}
