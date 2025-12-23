namespace Reserveit.Application.Common.DTOs.UserDtos;

public record UserDto
(
    Guid Id,
    string Email,
    string? FullName,
    string Role
);
