using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Activity.Application.Features.Activities;

public class DeleteActivityCommandHandler(IActivityRepository repository, ILogger<DeleteActivityCommandHandler> logger)
    : IRequestHandler<DeleteActivityCommand, Result>
{
    private readonly IActivityRepository _repository = repository;
    private readonly ILogger<DeleteActivityCommandHandler> _logger = logger;

    public async Task<Result> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetByIdAsync(request.Id);
        if (activity == null)
        {
            _logger.LogWarning("Attempt to delete non-existing Activity: {Id}", request.Id);
            return Result.Failure($"Activity with ID {request.Id} not found.");
        }

        await _repository.DeleteAsync(request.Id);
        _logger.LogInformation("Deleted Activity: {Id}", request.Id);
        return Result.Success();
    }
}