using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
namespace Reserveit.Domain.Interfaces;

public interface IReservationRepository
{
    // Queries
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Reservation>> GetByClientIdAsync(Guid clientId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<Reservation>> GetByBusinessIdAsync(Guid businessId, int page, int pageSize, CancellationToken cancellationToken);

    Task<Dictionary<ReservationStatus, int>> GetClientStatusCountsAsync(
    Guid clientId,
    CancellationToken ct);

    Task<(List<Reservation> Items, long TotalCount)> GetForBusinessRangeAsync(
        Guid businessId,
        DateTimeOffset from,
        DateTimeOffset to,
        ReservationStatus? status,
        Guid? staffId,
        int page,
        int pageSize,
        CancellationToken ct);

    Task<List<Reservation>> GetBlockingForStaffAsync(
        Guid staffId,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken ct);

    Task<List<Reservation>> GetForStaffRangeAsync(
        Guid staffId,
        DateTimeOffset from,
        DateTimeOffset to,
        ReservationStatus? status,
        CancellationToken ct);


    // staff today / upcoming
    Task<int> CountUpcomingForStaffAsync(
        Guid staffId,
        DateTimeOffset from,
        CancellationToken ct);

    Task<List<Reservation>> GetUpcomingForStaffAsync(
        Guid staffId,
        DateTimeOffset from,
        int page,
        int pageSize,
        CancellationToken ct);



    // check slot availability
    Task<bool> IsSlotAvailableAsync(Guid businessId, Guid? staffId, DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken);


    Task<bool> HasFutureForStaffAsync(Guid staffId, DateTimeOffset nowUtc, CancellationToken ct);

    // Commands
    Task AddAsync(Reservation reservation, CancellationToken cancellationToken);
    //Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken);
    Task DeleteAsync(Reservation reservation, CancellationToken cancellationToken);

    // Sacve changes
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
