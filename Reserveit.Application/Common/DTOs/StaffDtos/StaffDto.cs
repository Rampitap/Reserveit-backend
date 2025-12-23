namespace Reserveit.Application.Common.DTOs.StaffDtos;

public record StaffDto
(
    Guid Id,
    Guid BusinessId,
    string DisplayName,
    string? Bio,
    
    List<string> ServiceNames
);
