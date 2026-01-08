using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinesses;

public sealed record GetOwnerBusinessesQuery : IRequest<IReadOnlyList<OwnerBusinessSummaryDto>>;
