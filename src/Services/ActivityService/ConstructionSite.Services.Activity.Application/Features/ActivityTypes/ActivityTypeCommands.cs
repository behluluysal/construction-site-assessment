using ConstructionSite.Services.Activity.Application.DTOs;
using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Entities;
using MediatR;

namespace ConstructionSite.Services.Activity.Application.Features.ActivityTypes;

public record CreateActivityTypeCommand(string Name) : IRequest<Result<Ulid>>;
public record UpdateActivityTypeCommand(Ulid Id, string Name) : IRequest<Result>;
public record DeleteActivityTypeCommand(Ulid Id) : IRequest<Result>;
public record GetActivityTypeByIdQuery(Ulid Id) : IRequest<Result<ActivityType>>;
public record GetAllActivityTypesQuery(int PageNumber, int PageSize, string? OrderBy, string? Filter) : IRequest<Result<ActivityTypeListResultDto>>;