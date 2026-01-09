using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.DeleteAdminUser;

public sealed record DeleteAdminUserCommand(Guid UserId) : IRequest;
