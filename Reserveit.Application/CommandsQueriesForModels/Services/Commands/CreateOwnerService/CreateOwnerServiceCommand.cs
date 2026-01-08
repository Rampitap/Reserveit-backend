using MediatR;
using Reserveit.Application.Common.DTOs.ServiceDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.CreateOwnerService;

public sealed record CreateOwnerServiceCommand(Guid BusinessId, CreateServiceDto Data) : IRequest<Guid>;
