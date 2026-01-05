using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Interfaces;
using Reserveit.Infrastructure.Persistence;

namespace Reserveit.Infrastructure.Repositories;

public sealed class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _context;

    public ServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken ct)
    => _context.Services
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.Id == id && s.IsActive, ct);
}
