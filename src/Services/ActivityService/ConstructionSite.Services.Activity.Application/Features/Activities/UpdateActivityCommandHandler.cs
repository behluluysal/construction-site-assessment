using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.Activities;

public class UpdateActivityCommandHandler(IActivityRepository repository, ILogger<UpdateActivityCommandHandler> logger)
    : IRequestHandler<UpdateActivityCommand, Result>
{
    private readonly IActivityRepository _repository = repository;
    private readonly ILogger<UpdateActivityCommandHandler> _logger = logger;

    public async Task<Result> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetByIdAsync(request.Id);
        if (activity == null)
        {
            _logger.LogWarning("Attempt to update non-existing Activity: {Id}", request.Id);
            return Result.Failure($"Activity with ID {request.Id} not found.");
        }

        activity.ActivityDate = request.ActivityDate;
        activity.ActivityTypeId = request.ActivityTypeId;
        activity.Description = request.Description;
        activity.Worker = request.Worker;

        await _repository.UpdateAsync(activity);

        _logger.LogInformation("Updated Activity: {Id}", request.Id);
        return Result.Success();
    }
}