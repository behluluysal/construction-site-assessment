using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using ConstructionSite.Services.Activity.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.ActivityTypes;

public class GetActivityTypeByIdQueryHandler(IActivityTypeRepository repository, ILogger<GetActivityTypeByIdQueryHandler> logger)
    : IRequestHandler<GetActivityTypeByIdQuery, Result<ActivityType>>
{
    private readonly IActivityTypeRepository _repository = repository;
    private readonly ILogger<GetActivityTypeByIdQueryHandler> _logger = logger;

    public async Task<Result<ActivityType>> Handle(GetActivityTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var activityType = await _repository.GetByIdAsync(request.Id);
        if (activityType == null)
        {
            _logger.LogWarning("ActivityType not found: {Id}", request.Id);
            return Result<ActivityType>.FailureResult($"Activity Type with ID {request.Id} not found.");
        }

        return Result<ActivityType>.SuccessResult(activityType);
    }
}