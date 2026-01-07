using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;
using Reserveit.Infrastructure.Persistence;
using Reserveit.Domain.Interfaces;

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
}
