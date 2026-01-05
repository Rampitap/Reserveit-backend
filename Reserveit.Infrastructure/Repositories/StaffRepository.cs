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
}
