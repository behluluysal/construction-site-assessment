using ConstructionSite.Services.Activity.Domain.Contracts;
using ConstructionSite.Services.Activity.Domain.Entities;
using ConstructionSite.Services.Activity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ConstructionSite.Services.Activity.Infrastructure.Repositories;

public class ActivityTypeRepository(ActivityDbContext dbContext) : IActivityTypeRepository
{
    private readonly ActivityDbContext _dbContext = dbContext;

    public async Task<(IEnumerable<ActivityType>, int, int, int)> GetAllActivityTypesAsync(int pageNumber, int pageSize, string? orderBy, string? filter)
    {
        IQueryable<ActivityType> query = _dbContext.ActivityTypes.AsNoTracking();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();
        query = !string.IsNullOrEmpty(orderBy) ? query.OrderBy(orderBy) : query.OrderBy(a => a.Id);

        var activityTypes = await query.Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

        return (activityTypes, totalCount, activityTypes.Count, (int)Math.Ceiling((double)totalCount / pageSize));
    }

    public async Task<ActivityType?> GetByIdAsync(Ulid id) => await _dbContext.ActivityTypes.FindAsync(id);

    public async Task AddAsync(ActivityType activityType)
    {
        _dbContext.ActivityTypes.Add(activityType);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ActivityType activityType)
    {
        _dbContext.ActivityTypes.Update(activityType);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ulid id)
    {
        var activityType = await GetByIdAsync(id);
        if (activityType != null)
        {
            _dbContext.ActivityTypes.Remove(activityType);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ActivityType?> GetByNameAsync(string name) => await _dbContext.ActivityTypes.Where(x => x.Name == name).FirstOrDefaultAsync();

    public async Task<bool> AnyWithActivityTypeAsync(Ulid activityTypeId) => await _dbContext.Activities.AnyAsync(a => a.ActivityTypeId == activityTypeId);
}
