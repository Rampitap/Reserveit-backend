using Reserveit.Domain.Entities;

namespace Reserveit.Domain.Interfaces;

public interface IReservationRepository
{
    // Queries
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Reservation>> GetByClientIdAsync(Guid clientId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<Reservation>> GetByBusinessIdAsync(Guid businessId, int page, int pageSize, CancellationToken cancellationToken);

    // check slot availability
    Task<bool> IsSlotAvailableAsync(Guid businessId, Guid? staffId, DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken);

    // Commands
    Task AddAsync(Reservation reservation, CancellationToken cancellationToken);
    //Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken);
    Task DeleteAsync(Reservation reservation, CancellationToken cancellationToken);

    // Sacve changes
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
