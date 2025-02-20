namespace ConstructionSite.Services.Activity.Application.DTOs;

public class ActivityTypeListResultDto
{
    public IEnumerable<ActivityTypeDto> ActivityTypes { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int ElementsInCurrentPage { get; set; }
}