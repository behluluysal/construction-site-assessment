using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using ConstructionSite.Services.Activity.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.ActivityTypes;

public class CreateActivityTypeCommandHandler(IActivityTypeRepository repository, ILogger<CreateActivityTypeCommandHandler> logger)
    : IRequestHandler<CreateActivityTypeCommand, Result<Ulid>>
{
    private readonly IActivityTypeRepository _repository = repository;
    private readonly ILogger<CreateActivityTypeCommandHandler> _logger = logger;

    public async Task<Result<Ulid>> Handle(CreateActivityTypeCommand request, CancellationToken cancellationToken)
    {
        var existingType = await _repository.GetByNameAsync(request.Name);
        if (existingType != null)
        {
            _logger.LogWarning("Attempt to create duplicate ActivityType: {Name}", request.Name);
            return Result<Ulid>.FailureResult($"Activity Type '{request.Name}' already exists.");
        }

        var activityType = new ActivityType { Name = request.Name };

        await _repository.AddAsync(activityType);

        _logger.LogInformation("Created ActivityType: {Name} with ID {Id}", activityType.Name, activityType.Id);

        return Result<Ulid>.SuccessResult(activityType.Id);
    }
}