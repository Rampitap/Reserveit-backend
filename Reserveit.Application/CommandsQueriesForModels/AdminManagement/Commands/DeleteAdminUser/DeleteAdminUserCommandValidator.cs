using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.DeleteAdminUser;

public sealed class DeleteAdminUserCommandValidator : AbstractValidator<DeleteAdminUserCommand>
{
    public DeleteAdminUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
