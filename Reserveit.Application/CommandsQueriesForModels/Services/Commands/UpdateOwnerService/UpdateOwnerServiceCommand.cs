using MediatR;
using Reserveit.Application.Common.DTOs.ServiceDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.UpdateOwnerService;

public sealed record UpdateOwnerServiceCommand(Guid BusinessId, Guid ServiceId, UpdateServiceDto Data) : IRequest;
