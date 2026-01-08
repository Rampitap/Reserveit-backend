using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.DeactivateOwnerService;

public sealed record DeactivateOwnerServiceCommand(Guid BusinessId, Guid ServiceId) : IRequest;
