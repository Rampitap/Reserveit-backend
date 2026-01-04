namespace Reserveit.Application.Common.DTOs.UserDtos;

public sealed class UpdateProfileDto
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
}
