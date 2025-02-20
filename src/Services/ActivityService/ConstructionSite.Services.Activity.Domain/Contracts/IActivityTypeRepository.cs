using ConstructionSite.Services.Activity.Domain.Entities;

namespace ConstructionSite.Services.Activity.Domain.Contracts;

public interface IActivityTypeRepository
{
    Task<(IEnumerable<ActivityType>, int, int, int)> GetAllActivityTypesAsync(int pageNumber, int pageSize, string? orderBy, string? filter);
    Task<ActivityType?> GetByIdAsync(Ulid id);
    Task AddAsync(ActivityType activityType);
    Task UpdateAsync(ActivityType activityType);
    Task DeleteAsync(Ulid id);

    Task<ActivityType?> GetByNameAsync(string name);
    Task<bool> AnyWithActivityTypeAsync(Ulid activityTypeId);
}