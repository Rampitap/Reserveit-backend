using MediatR;
using Reserveit.Application.Common.DTOs.ServiceDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessServices;

public sealed record GetPublicBusinessServicesQuery(Guid BusinessId) : IRequest<IReadOnlyList<PublicServiceDto>>;
