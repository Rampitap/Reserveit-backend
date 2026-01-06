using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusiness;

public sealed record GetPublicBusinessDetailsQuery(Guid BusinessId) : IRequest<PublicBusinessDetailsDto>;
