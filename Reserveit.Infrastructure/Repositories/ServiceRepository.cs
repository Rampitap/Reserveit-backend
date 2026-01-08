using Microsoft.EntityFrameworkCore;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;
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

    public Task<List<Service>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct)
    => _context.Services.AsNoTracking()
        .Where(s => s.BusinessId == businessId)
        .OrderBy(s => s.Name)
        .ToListAsync(ct);

    public Task<Service?> GetTrackedByIdAsync(Guid id, CancellationToken ct)
        => _context.Services
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task AddAsync(Service service, CancellationToken ct)
       => _context.Services.AddAsync(service, ct).AsTask();

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);

    public async Task AddStaffToServiceAsync(Guid businessId, Guid serviceId, Guid staffId, CancellationToken ct)
    {
        var service = await _context.Services
            .Include(s => s.Staffs)
            .FirstOrDefaultAsync(s => s.Id == serviceId, ct)
            ?? throw new NotFoundException("Service", serviceId.ToString());

        if (service.BusinessId != businessId)
            throw new ForbiddenException("Service doesn't belong to this business.");

        var staff = await _context.Staffs
            .FirstOrDefaultAsync(s => s.Id == staffId, ct)
            ?? throw new NotFoundException("Staff", staffId.ToString());

        if (staff.BusinessId != businessId)
            throw new ForbiddenException("Staff doesn't belong to this business.");

        if (!service.IsActive) throw new InvalidOperationException("Service is inactive.");
        if (!staff.IsActive) throw new InvalidOperationException("Staff is inactive.");

        if (service.Staffs.Any(x => x.Id == staffId))
            return;

        service.Staffs.Add(staff);
        service.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoveStaffFromServiceAsync(Guid businessId, Guid serviceId, Guid staffId, CancellationToken ct)
    {
        var service = await _context.Services
            .Include(s => s.Staffs)
            .FirstOrDefaultAsync(s => s.Id == serviceId, ct)
            ?? throw new NotFoundException("Service", serviceId.ToString());

        if (service.BusinessId != businessId)
            throw new ForbiddenException("Service doesn't belong to this business.");

        var staff = service.Staffs.FirstOrDefault(x => x.Id == staffId);
        if (staff == null)
            return;

        service.Staffs.Remove(staff);
        service.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(ct);
    }
}

