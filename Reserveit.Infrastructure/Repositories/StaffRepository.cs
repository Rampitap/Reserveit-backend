using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Interfaces;
using Reserveit.Infrastructure.Persistence;

namespace Reserveit.Infrastructure.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _db;
    public StaffRepository(AppDbContext db) => _db = db;

    public Task<Staff?> GetByUserIdAsync(Guid userId, CancellationToken ct)
        => _db.Staffs.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId, ct);

    public Task<Staff?> GetByIdAsync(Guid staffId, CancellationToken ct)
    => _db.Staffs
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive, ct);

    public Task<List<Staff>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct)
       => _db.Staffs
           .AsNoTracking()
           .Include(s => s.User)
           .Where(s => s.BusinessId == businessId)
           .OrderByDescending(s => s.IsActive)
           .ThenBy(s => s.DisplayName)
           .ToListAsync(ct);

    public Task<Staff?> GetByBusinessAndIdAsync(Guid businessId, Guid staffId, CancellationToken ct)
        => _db.Staffs
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.BusinessId == businessId && s.Id == staffId, ct);


    public Task<Staff?> GetTrackedByBusinessAndIdAsync(Guid businessId, Guid staffId, CancellationToken ct)
    => _db.Staffs.FirstOrDefaultAsync(s => s.BusinessId == businessId && s.Id == staffId, ct);

    public Task DeleteAsync(Staff staff, CancellationToken ct)
    {
        _db.Staffs.Remove(staff);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
