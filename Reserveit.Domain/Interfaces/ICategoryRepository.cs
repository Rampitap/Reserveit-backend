using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;

namespace Reserveit.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken ct);
    Task<Dictionary<ReservationStatus, int>> GetClientStatusCountsAsync(
    Guid clientId,
    CancellationToken ct);
}
