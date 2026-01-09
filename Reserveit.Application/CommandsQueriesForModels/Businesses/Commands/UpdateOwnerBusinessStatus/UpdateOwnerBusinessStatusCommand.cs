using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusinessStatus;

public sealed record UpdateOwnerBusinessStatusCommand(Guid BusinessId, bool IsActive) : IRequest;
