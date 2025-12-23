namespace Reserveit.Application.Common.DTOs.StaffDtos;

public record CreateStaffDto
(
    string DisplayName,
    string? Bio,
    string? Email,
    
    List<Guid> ServiceIds
);
