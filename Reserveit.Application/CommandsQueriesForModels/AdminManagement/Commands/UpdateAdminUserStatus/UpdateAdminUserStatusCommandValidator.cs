using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.UpdateAdminUserStatus;

public sealed class UpdateAdminUserStatusCommandValidator : AbstractValidator<UpdateAdminUserStatusCommand>
{
    public UpdateAdminUserStatusCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
