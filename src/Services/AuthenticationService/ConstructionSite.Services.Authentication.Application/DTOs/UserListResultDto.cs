namespace ConstructionSite.Services.Authentication.Application.DTOs;

public class UserListResultDto
{
    public IEnumerable<UserDto> Users { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int ElementsInCurrentPage { get; set; }
}
