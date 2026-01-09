using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusiness;

public sealed record UpdateOwnerBusinessCommand(Guid BusinessId, UpdateBusinessDto Data) : IRequest;
