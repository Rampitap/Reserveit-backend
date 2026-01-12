using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Interfaces;
using Reserveit.Infrastructure.Persistence;

namespace Reserveit.Infrastructure.Repositories;

public sealed class BusinessRepository : IBusinessRepository
{
    private readonly AppDbContext _db;
    public BusinessRepository(AppDbContext db) => _db = db;

    public Task<Business?> GetPublicByIdAsync(Guid businessId, CancellationToken ct)
        => _db.Businesses
            .AsNoTracking()
            .Where(b => b.Id == businessId && b.IsActive)
            .Include(b => b.Services.Where(s => s.IsActive))
                .ThenInclude(s => s.Staffs.Where(st => st.IsActive))
            .Include(b => b.StaffMembers.Where(st => st.IsActive))
                .ThenInclude(st => st.Services.Where(s => s.IsActive))
            .FirstOrDefaultAsync(ct);

    public async Task<List<Service>> GetPublicServicesAsync(Guid businessId, CancellationToken ct)
        => await _db.Services
            .AsNoTracking()
            .Where(s => s.BusinessId == businessId && s.IsActive)
            .Include(s => s.Staffs.Where(st => st.IsActive))
            .OrderBy(s => s.Name)
            .ToListAsync(ct);

    public async Task<List<Staff>> GetPublicStaffAsync(Guid businessId, CancellationToken ct)
        => await _db.Staffs
            .AsNoTracking()
            .Where(st => st.BusinessId == businessId && st.IsActive)
            .Include(st => st.Services.Where(s => s.IsActive))
            .OrderBy(st => st.DisplayName)
            .ToListAsync(ct);

    public async Task<List<Business>> SearchPublicAsync(int page, int pageSize, string? q, CancellationToken ct)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 12 : pageSize;

        var query = _db.Businesses
            .AsNoTracking()
            .Where(b => b.IsActive);

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(b =>
                b.Name.Contains(q) ||
                (b.Address != null && b.Address.Contains(q)));
        }

        return await query
            .OrderBy(b => b.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountPublicAsync(string? q, CancellationToken ct)
    {
        var query = _db.Businesses.AsNoTracking().Where(b => b.IsActive);

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(b =>
                b.Name.Contains(q) ||
                (b.Address != null && b.Address.Contains(q)));
        }

        return await query.CountAsync(ct);
    }

    public Task<bool> IsOwnedByAsync(Guid businessId, Guid ownerId, CancellationToken ct)
    {
        return _db.Businesses
            .AsNoTracking()
            .AnyAsync(b =>
                b.Id == businessId &&
                b.OwnerId == ownerId &&
                b.IsActive,
                ct);
    }

    public Task<List<Business>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct)
    => _db.Businesses
        .AsNoTracking()
        .Where(b => b.OwnerId == ownerId)
        .OrderBy(b => b.Name)
        .ToListAsync(ct);

    public Task<Business?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Businesses.FirstOrDefaultAsync(b => b.Id == id, ct);


    public async Task AddAsync(Business business, CancellationToken ct)
        => await _db.Businesses.AddAsync(business, ct);

    public Task DeleteAsync(Business business, CancellationToken ct)
    {
        _db.Businesses.Remove(business);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);

    public Task<bool> HasFutureConfirmedReservationsAsync(Guid businessId, DateTimeOffset nowUtc, CancellationToken ct)
      => _db.Reservations.AsNoTracking()
          .AnyAsync(r =>
              r.BusinessId == businessId &&
              r.Status == ReservationStatus.Confirmed &&
              r.StartAt > nowUtc,
              ct);

    public async Task<List<Business>> GetPublicByCategoryAsync(
    int page,
    int pageSize,
    string? q,
    Guid? categoryId,
    string? categoryName,
    CancellationToken ct)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 12 : pageSize;

        IQueryable<Business> query = _db.Businesses
            .AsNoTracking()
            .Where(b => b.IsActive);

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(categoryName))
        {
            var cat = categoryName.Trim();
            query = query.Where(b => b.Category != null && b.Category.Name == cat);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(b =>
                b.Name.Contains(term) ||
                (b.Address != null && b.Address.Contains(term)));
        }

        query = query.Include(b => b.Category);

        return await query
            .OrderBy(b => b.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountPublicByCategoryAsync(
        string? q, Guid? categoryId, string? categoryName, CancellationToken ct)
    {
        var query = _db.Businesses.AsNoTracking()
            .Where(b => b.IsActive);

        if (categoryId.HasValue)
            query = query.Where(b => b.CategoryId == categoryId.Value);
        else if (!string.IsNullOrWhiteSpace(categoryName))
            query = query.Where(b => b.Category != null && b.Category.Name == categoryName.Trim());

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(b => b.Name.Contains(term) || (b.Address != null && b.Address.Contains(term)));
        }

        return await query.CountAsync(ct);
    }
}
