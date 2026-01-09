using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinessById;

public sealed class GetOwnerBusinessByIdQueryHandler
    : IRequestHandler<GetOwnerBusinessByIdQuery, OwnerBusinessDetailsDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _repo;
    private readonly IValidator<GetOwnerBusinessByIdQuery> _validator;
    private readonly ILogger<GetOwnerBusinessByIdQueryHandler> _logger;

    public GetOwnerBusinessByIdQueryHandler(
        ICurrentUser currentUser,
        IBusinessRepository repo,
        IValidator<GetOwnerBusinessByIdQuery> validator,
        ILogger<GetOwnerBusinessByIdQueryHandler> logger)
    {
        _currentUser = currentUser;
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task<OwnerBusinessDetailsDto> Handle(GetOwnerBusinessByIdQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var business = await _repo.GetByIdAsync(request.BusinessId, ct)
            ?? throw new NotFoundException(nameof(Business), request.BusinessId.ToString());

        if (business.OwnerId != _currentUser.UserId)
            throw new ForbiddenException("You don't have access to this business.");

        _logger.LogInformation("Owner fetched business details. BusinessId={BusinessId}, OwnerId={OwnerId}",
            business.Id, business.OwnerId);

        return new OwnerBusinessDetailsDto
        {
            Id = business.Id,
            Name = business.Name,
            Address = business.Address,
            Timezone = business.Timezone,
            OpeningTime = business.OpeningTime,
            ClosingTime = business.ClosingTime,
            CancellationPolicyJson = business.CancellationPolicyJson,
            IsActive = business.IsActive,
            CreatedAt = business.CreatedAt,
            UpdatedAt = business.UpdatedAt
        };
    }
}
