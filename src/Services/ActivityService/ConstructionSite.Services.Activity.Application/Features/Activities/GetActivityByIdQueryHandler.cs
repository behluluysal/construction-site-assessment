using ConstructionSite.Services.Activity.Application.DTOs;
using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.Activities;

public class GetActivityByIdQueryHandler(IActivityRepository repository, ILogger<GetActivityByIdQueryHandler> logger)
    : IRequestHandler<GetActivityByIdQuery, Result<ActivityDto>>
{
    private readonly IActivityRepository _repository = repository;
    private readonly ILogger<GetActivityByIdQueryHandler> _logger = logger;

    public async Task<Result<ActivityDto>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetByIdAsync(request.Id);
        if (activity == null)
        {
            _logger.LogWarning("Activity with ID {Id} not found", request.Id);
            return Result<ActivityDto>.FailureResult($"Activity with ID {request.Id} not found.");
        }
        var data = new ActivityDto(activity.Id, activity.ActivityDate, new(activity.ActivityType.Id, activity.ActivityType.Name), activity.Description, activity.Worker);
        return Result<ActivityDto>.SuccessResult(data);
    }
}
