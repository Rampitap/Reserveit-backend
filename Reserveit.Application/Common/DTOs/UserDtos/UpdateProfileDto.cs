namespace Reserveit.Application.Common.DTOs.UserDtos;

public record UpdateProfileDto
(
     string FirstName,
     string LastName,
     string? Phone
);
