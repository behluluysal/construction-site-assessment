using ConstructionSite.Services.Activity.Application.DTOs;
using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.Activities;

public class GetAllActivitiesQueryHandler(IActivityRepository repository, ILogger<GetAllActivitiesQueryHandler> logger)
    : IRequestHandler<GetAllActivitiesQuery, Result<ActivityListResultDto>>
{
    private readonly IActivityRepository _repository = repository;
    private readonly ILogger<GetAllActivitiesQueryHandler> _logger = logger;

    public async Task<Result<ActivityListResultDto>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching activities. Page: {PageNumber}, Size: {PageSize}, Filter: {Filter}",
            request.PageNumber, request.PageSize, request.Filter ?? "None");

        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            _logger.LogWarning("Invalid pagination parameters. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);
            return Result<ActivityListResultDto>.FailureResult(["Invalid pagination parameters."]);
        }

        var (activities, totalCount, elementsInCurrentPage, totalPages) =
            await _repository.GetAllActivitiesAsync(request.PageNumber, request.PageSize, request.OrderBy, request.Filter);

        var activityDtos = activities.Select(a => new ActivityDto(a.Id, a.ActivityDate, new(a.ActivityTypeId, a.ActivityType.Name), a.Description, a.Worker)).ToList();

        var result = new ActivityListResultDto
        {
            Activities = activityDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = totalPages,
            ElementsInCurrentPage = elementsInCurrentPage
        };

        return Result<ActivityListResultDto>.SuccessResult(result);
    }
}