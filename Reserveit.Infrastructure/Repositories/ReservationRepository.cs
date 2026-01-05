using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Interfaces;
using Reserveit.Infrastructure.Persistence;

namespace Reserveit.Infrastructure.Repositories;

public class ReservationRepository: IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }
    #region queries
    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken ct) 
    {
        return await _context.Reservations
            .Include(r => r.Service)
            .Include(r => r.Business)
            .Include(r => r.Staff)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

   public async Task<List<Reservation>> GetByClientIdAsync(Guid clientId, int page, int pageSize, CancellationToken ct) 
   {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        return await _context.Reservations
            .AsNoTracking()
            .Where(r => r.ClientId == clientId)
            .OrderByDescending(r => r.StartAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.Service)
            .Include(r => r.Business)
            .Include(r => r.Staff)
            .Include(r => r.Client)
            .ToListAsync(ct);
    }

    public async Task<List<Reservation>> GetByBusinessIdAsync(Guid businessId, int page, int pageSize, CancellationToken ct) 
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        return await _context.Reservations
            .AsNoTracking()
            .Where(r => r.BusinessId == businessId)
            .OrderByDescending(r => r.StartAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.Service)
            .Include(r => r.Client)
            .Include(r => r.Staff)
            .ToListAsync(ct);
    }

    public async Task<List<Reservation>> GetForStaffRangeAsync(
    Guid staffId,
    DateTimeOffset from,
    DateTimeOffset to,
    ReservationStatus? status,
    CancellationToken ct)
    {
        var q = _context.Reservations
        .AsNoTracking()
        .Include(r => r.Business)
        .Include(r => r.Service)
        .Include(r => r.Staff)
        .Include(r => r.Client)
        .Where(r => r.StaffId == staffId)
        .Where(r => r.StartAt >= from && r.StartAt < to);

        if (status.HasValue)
            q = q.Where(r => r.Status == status.Value);

        return await q
            .OrderBy(r => r.StartAt)
            .ToListAsync(ct);
    }
    #endregion


    public async Task<int> CountUpcomingForStaffAsync(
    Guid staffId,
    DateTimeOffset from,
    CancellationToken ct)
    {
        return await _context.Reservations
            .Where(r =>
                r.StaffId == staffId &&
                r.StartAt >= from &&
                (r.Status == ReservationStatus.Pending ||
                 r.Status == ReservationStatus.Confirmed))
            .CountAsync(ct);
    }

    public async Task<List<Reservation>> GetUpcomingForStaffAsync(
    Guid staffId,
    DateTimeOffset from,
    int page,
    int pageSize,
    CancellationToken ct)
    {
        return await _context.Reservations
            .Where(r =>
                r.StaffId == staffId &&
                r.StartAt >= from &&
                (r.Status == ReservationStatus.Pending ||
                 r.Status == ReservationStatus.Confirmed))
            .OrderBy(r => r.Status == ReservationStatus.Confirmed ? 0 : 1)
            .ThenBy(r => r.StartAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    //повернутися до редагування пізніше
    public async Task<bool> IsSlotAvailableAsync(
    Guid businessId,
    Guid? staffId,
    DateTimeOffset start,
    DateTimeOffset end,
    CancellationToken ct)
    {
        var query = _context.Reservations
            .AsNoTracking()
            .Where(r =>
                r.BusinessId == businessId &&
                r.StartAt < end &&
                r.EndAt > start)
            .Where(r => r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed);

        if (staffId.HasValue)
            query = query.Where(r => r.StaffId == staffId.Value);

        return !await query.AnyAsync(ct);
    }

    #region commands

    public async Task AddAsync(Reservation reservation, CancellationToken ct) 
    {
        if (reservation is null) throw new ArgumentNullException(nameof(reservation));
        await _context.Reservations.AddAsync(reservation, ct);
    }

    


    public Task DeleteAsync(Reservation reservation, CancellationToken cancellationToken) 
    {
        if (reservation is null) throw new ArgumentNullException(nameof(reservation));
        _context.Reservations.Remove(reservation);
        return Task.CompletedTask;
    }
    #endregion

    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}
