using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinessById;

public sealed record GetOwnerBusinessByIdQuery(Guid BusinessId) : IRequest<OwnerBusinessDetailsDto>;
