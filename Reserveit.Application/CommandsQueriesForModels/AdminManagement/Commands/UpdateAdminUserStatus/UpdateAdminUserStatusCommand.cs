using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.UpdateAdminUserStatus;

public sealed record UpdateAdminUserStatusCommand(Guid UserId, bool IsActive) : IRequest;
