using FluentValidation;

namespace Reserveit.Application.Profiles.Commands.UpdateMyProfile;

public sealed class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        RuleFor(x => x.Data.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Data.LastName).NotEmpty().MaximumLength(100);
    }
}