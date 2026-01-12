using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Interfaces;
using Reserveit.Infrastructure.Persistence;

namespace Reserveit.Infrastructure.Repositories;
public sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;
    public CategoryRepository(AppDbContext db) => _db = db;

    public Task<List<Category>> GetAllAsync(CancellationToken ct)
        => _db.Categories.AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    public async Task<Dictionary<ReservationStatus, int>> GetClientStatusCountsAsync(Guid clientId, CancellationToken ct)
    {
        return await _db.Reservations
            .AsNoTracking()
            .Where(r => r.ClientId == clientId)
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count, ct);
    }
}
