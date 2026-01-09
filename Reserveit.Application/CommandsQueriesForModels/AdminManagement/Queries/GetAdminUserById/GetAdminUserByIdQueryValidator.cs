using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUserById;

public sealed class GetAdminUserByIdQueryValidator : AbstractValidator<GetAdminUserByIdQuery>
{
    public GetAdminUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
