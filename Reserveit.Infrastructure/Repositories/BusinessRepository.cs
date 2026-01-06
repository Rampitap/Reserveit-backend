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
}
