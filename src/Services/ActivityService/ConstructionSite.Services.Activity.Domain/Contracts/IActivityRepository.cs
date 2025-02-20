namespace ConstructionSite.Services.Activity.Domain.Contracts;

public interface IActivityRepository
{
    Task<(IEnumerable<Entities.Activity>, int, int, int)> GetAllActivitiesAsync(int pageNumber, int pageSize, string? orderBy, string? filter);
    Task<Entities.Activity?> GetByIdAsync(Ulid id);
    Task AddAsync(Entities.Activity activity);
    Task UpdateAsync(Entities.Activity activity);
    Task DeleteAsync(Ulid id);
}
