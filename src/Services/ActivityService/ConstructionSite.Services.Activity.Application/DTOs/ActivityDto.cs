namespace ConstructionSite.Services.Activity.Application.DTOs;

public record ActivityDto(Ulid Id, DateTime ActivityDate, ActivityTypeDto ActivityType, string Description, string Worker);

public class ActivityListResultDto
{
    public List<ActivityDto> Activities { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int ElementsInCurrentPage { get; set; }
}