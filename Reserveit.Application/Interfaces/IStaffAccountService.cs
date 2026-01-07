using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.Interfaces;

public interface IStaffAccountService
{
    Task<Guid> CreateStaffAsync(CreateStaffAccountDto dto, CancellationToken ct);
}
