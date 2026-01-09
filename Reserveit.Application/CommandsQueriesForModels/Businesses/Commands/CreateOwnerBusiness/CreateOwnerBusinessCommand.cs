using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.CreateOwnerBusiness;

public sealed record CreateOwnerBusinessCommand(CreateBusinessDto Data) : IRequest<Guid>;
