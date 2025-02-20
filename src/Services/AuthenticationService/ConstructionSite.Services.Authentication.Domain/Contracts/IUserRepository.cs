using ConstructionSite.Services.Authentication.Domain.Entities;

namespace ConstructionSite.Services.Authentication.Domain.Contracts;

public interface IUserRepository
{
    Task<(IEnumerable<ApplicationUser>, int, int, int)> GetAllUsersAsync(int pageNumber, int pageSize, string? orderBy, string? filter);
}
