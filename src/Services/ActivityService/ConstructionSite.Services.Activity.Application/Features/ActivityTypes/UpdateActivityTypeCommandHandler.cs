using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConstructionSite.Services.Activity.Application.Features.ActivityTypes;

public class UpdateActivityTypeCommandHandler(IActivityTypeRepository repository, ILogger<UpdateActivityTypeCommandHandler> logger)
    : IRequestHandler<UpdateActivityTypeCommand, Result>
{
    private readonly IActivityTypeRepository _repository = repository;
    private readonly ILogger<UpdateActivityTypeCommandHandler> _logger = logger;

    public async Task<Result> Handle(UpdateActivityTypeCommand request, CancellationToken cancellationToken)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var activityType = await _repository.GetByIdAsync(request.Id);
        stopWatch.Stop();
        _logger.LogWarning("ActivityType fetched in {Ms} ms", stopWatch.ElapsedMilliseconds);

        if (activityType == null)
        {
            _logger.LogWarning("Attempt to update non-existing ActivityType: {Id}", request.Id);
            return Result.Failure($"Activity Type with ID {request.Id} not found.");
        }

        stopWatch.Restart();
        var existingType = await _repository.GetByNameAsync(request.Name);
        _logger.LogWarning("existingType fetched in {Ms} ms", stopWatch.ElapsedMilliseconds);

        if (existingType != null && existingType.Id != request.Id)
        {
            _logger.LogWarning("Attempt to rename ActivityType to an existing name: {Name}", request.Name);
            return Result.Failure($"Activity Type '{request.Name}' already exists.");
        }

        activityType.Name = request.Name;
        stopWatch.Restart();
        await _repository.UpdateAsync(activityType);
        _logger.LogWarning("Type updated in {Ms} ms", stopWatch.ElapsedMilliseconds);

        _logger.LogInformation("Updated ActivityType: {Id} with new name {Name}", request.Id, request.Name);

        return Result.Success();
    }
}
