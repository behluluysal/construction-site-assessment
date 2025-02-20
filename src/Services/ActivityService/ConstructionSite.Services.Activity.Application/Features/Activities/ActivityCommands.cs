using ConstructionSite.Services.Activity.Application.DTOs;
using ConstructionSite.Services.Activity.Domain.Common;
using MediatR;

namespace ConstructionSite.Services.Activity.Application.Features.Activities;

public record CreateActivityCommand(DateTime ActivityDate, Ulid ActivityTypeId, string Description, string Worker)
    : IRequest<Result<Ulid>>;
public record UpdateActivityCommand(Ulid Id, DateTime ActivityDate, Ulid ActivityTypeId, string Description, string Worker) : IRequest<Result>;
public record DeleteActivityCommand(Ulid Id) : IRequest<Result>;
public record GetActivityByIdQuery(Ulid Id) : IRequest<Result<ActivityDto>>;
public record GetAllActivitiesQuery(int PageNumber, int PageSize, string? OrderBy, string? Filter)
    : IRequest<Result<ActivityListResultDto>>;
