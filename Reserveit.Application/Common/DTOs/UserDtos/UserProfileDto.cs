namespace Reserveit.Application.Common.DTOs.UserDtos;

public record UserProfileDto
(
    Guid Id,
    string Email,
    string? FullName,
    string Role
);
