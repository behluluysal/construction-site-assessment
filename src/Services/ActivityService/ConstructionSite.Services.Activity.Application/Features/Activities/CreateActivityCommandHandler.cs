using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.Activities;

public class CreateActivityHandler(IActivityRepository repository, ILogger<CreateActivityHandler> logger)
    : IRequestHandler<CreateActivityCommand, Result<Ulid>>
{
    private readonly IActivityRepository _repository = repository;
    private readonly ILogger<CreateActivityHandler> _logger = logger;

    public async Task<Result<Ulid>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
    {
        var activity = new Domain.Entities.Activity
        {
            Id = Ulid.NewUlid(),
            ActivityDate = request.ActivityDate,
            ActivityTypeId = request.ActivityTypeId,
            Description = request.Description,
            Worker = request.Worker
        };

        await _repository.AddAsync(activity);

        _logger.LogInformation("Created Activity: {Id}", activity.Id);

        return Result<Ulid>.SuccessResult(activity.Id);
    }
}
