using FluentValidation;

namespace Reserveit.Application.Profiles.Commands.DeleteMyProfile;

public sealed class DeleteMyProfileCommandValidator : AbstractValidator<DeleteMyProfileCommand>
{
    public DeleteMyProfileCommandValidator()
    {
        RuleFor(x => x.Data.Password).NotEmpty();
    }
}
