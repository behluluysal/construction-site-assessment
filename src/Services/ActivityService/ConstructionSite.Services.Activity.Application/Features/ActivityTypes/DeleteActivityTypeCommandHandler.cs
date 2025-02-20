using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.ActivityTypes;

public class DeleteActivityTypeCommandHandler(IActivityTypeRepository repository, ILogger<DeleteActivityTypeCommandHandler> logger)
    : IRequestHandler<DeleteActivityTypeCommand, Result>
{
    private readonly IActivityTypeRepository _repository = repository;
    private readonly ILogger<DeleteActivityTypeCommandHandler> _logger = logger;

    public async Task<Result> Handle(DeleteActivityTypeCommand request, CancellationToken cancellationToken)
    {
        var existingType = await _repository.GetByIdAsync(request.Id);
        if (existingType == null)
        {
            _logger.LogWarning("Attempt to delete non-existing ActivityType: {Id}", request.Id);
            return Result.Failure($"Activity Type with ID {request.Id} not found.");
        }

        var isUsedInActivities = await _repository.AnyWithActivityTypeAsync(request.Id);
        if (isUsedInActivities)
        {
            _logger.LogWarning("Attempt to delete ActivityType {Id} which is in use", request.Id);
            return Result.Failure($"Activity Type with ID {request.Id} is being used in activities and cannot be deleted.");
        }

        await _repository.DeleteAsync(request.Id);
        _logger.LogInformation("Deleted ActivityType: {Id}", request.Id);
        return Result.Success();
    }
}