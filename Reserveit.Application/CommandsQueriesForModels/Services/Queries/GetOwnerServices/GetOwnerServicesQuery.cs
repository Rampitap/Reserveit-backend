using MediatR;
using Reserveit.Application.Common.DTOs.ServiceDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Queries.GetOwnerServices;

public sealed record GetOwnerServicesQuery(Guid BusinessId) : IRequest<IReadOnlyList<OwnerServiceDto>>;
