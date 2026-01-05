using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;
using System.Security.Principal;

namespace Reserveit.Application.CommandsQueriesForModels.Profiles.Commands.UpdateMyPassword;

public sealed class UpdateMyPasswordCommandHandler : IRequestHandler<UpdateMyPasswordCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProfileService _profileService;
    private readonly ILogger<UpdateMyPasswordCommandHandler> _logger;
    private readonly IValidator<UpdateMyPasswordCommand> _validator;
    public UpdateMyPasswordCommandHandler(
        ICurrentUser currentUser,
        IProfileService profileService,
        ILogger<UpdateMyPasswordCommandHandler> logger,
        IValidator<UpdateMyPasswordCommand> validator)
    {
        _currentUser = currentUser;
        _profileService = profileService;
        _logger = logger;
        _validator = validator;
    }

    public async Task Handle(UpdateMyPasswordCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
        {
            _logger.LogWarning("UpdateMyPassword validation failed. UserId={UserId}. Errors={Errors}",
                _currentUser.UserId,
                string.Join("; ", vr.Errors.Select(e => $"{e.PropertyName}:{e.ErrorMessage}")));

            throw new ValidationException(vr.Errors);
        }

        var userId = _currentUser.UserId;

        await _profileService.ChangePasswordAsync(userId, request.Data, ct);

        _logger.LogInformation("ChangeMyPassword command processed. UserId={UserId}", userId);
    }
}
