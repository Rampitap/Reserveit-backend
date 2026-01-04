namespace Reserveit.Application.Common.DTOs.UserDtos;

public sealed class ChangePasswordRequestDto
{
    public string CurrentPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}
