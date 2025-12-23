namespace Reserveit.Application.Common.DTOs.UserDtos;

public record CreateUserDto
(
    string Email,
    string Password,
    string? FullName
);
