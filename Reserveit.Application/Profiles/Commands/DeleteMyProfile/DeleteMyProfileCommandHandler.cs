using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using System.Security.Principal;

namespace Reserveit.Application.Profiles.Commands.DeleteMyProfile;

public sealed class DeleteMyProfileCommandHandler : IRequestHandler<DeleteMyProfileCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProfileService _profileService;
    private readonly ILogger<DeleteMyProfileCommandHandler> _logger;
    private readonly IValidator<DeleteMyProfileCommand> _validator;

    public DeleteMyProfileCommandHandler(ICurrentUser currentUser, IProfileService profileService, ILogger<DeleteMyProfileCommandHandler> logger,
        IValidator<DeleteMyProfileCommand> validator)
    {
        _currentUser = currentUser;
        _profileService = profileService;
        _logger = logger;
        _validator = validator;
    }

    public async Task Handle(DeleteMyProfileCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
        {
            _logger.LogWarning("DeleteMyProfile validation failed. UserId={UserId}. Errors={Errors}",
                _currentUser.UserId,
                string.Join("; ", vr.Errors.Select(e => $"{e.PropertyName}:{e.ErrorMessage}")));

            throw new ValidationException(vr.Errors);
        }

        var userId = _currentUser.UserId;

        await _profileService.DeleteAccountAsync(userId, request.Data, ct);

        _logger.LogInformation("DeleteMyAccount command processed. UserId={UserId}", userId);
    }
}
