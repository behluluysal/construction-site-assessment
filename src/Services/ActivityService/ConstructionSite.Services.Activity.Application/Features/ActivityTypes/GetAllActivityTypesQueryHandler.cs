using ConstructionSite.Services.Activity.Application.DTOs;
using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.ActivityTypes;


public class GetAllActivityTypesQueryHandler(IActivityTypeRepository repository, ILogger<GetAllActivityTypesQueryHandler> logger)
    : IRequestHandler<GetAllActivityTypesQuery, Result<ActivityTypeListResultDto>>
{
    private readonly IActivityTypeRepository _repository = repository;
    private readonly ILogger<GetAllActivityTypesQueryHandler> _logger = logger;

    public async Task<Result<ActivityTypeListResultDto>> Handle(GetAllActivityTypesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching activity types. Page: {PageNumber}, Size: {PageSize}, Filter: {Filter}",
            request.PageNumber, request.PageSize, request.Filter ?? "None");

        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            _logger.LogWarning("Invalid pagination parameters. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);
            return Result<ActivityTypeListResultDto>.FailureResult(["Invalid pagination parameters."]);
        }

        var (activityTypes, totalCount, elementsInCurrentPage, totalPages) =
            await _repository.GetAllActivityTypesAsync(request.PageNumber, request.PageSize, request.OrderBy, request.Filter);

        var activityTypeDtos = activityTypes.Select(a => new ActivityTypeDto(a.Id, a.Name)).ToList();

        var result = new ActivityTypeListResultDto
        {
            ActivityTypes = activityTypeDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = totalPages,
            ElementsInCurrentPage = elementsInCurrentPage
        };

        return Result<ActivityTypeListResultDto>.SuccessResult(result);
    }
}

