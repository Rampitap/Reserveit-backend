using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.DeleteOwnerBusiness;

public sealed record DeleteOwnerBusinessCommand(Guid BusinessId) : IRequest;
