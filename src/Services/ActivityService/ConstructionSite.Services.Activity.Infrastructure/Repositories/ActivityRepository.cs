using ConstructionSite.Services.Activity.Domain.Contracts;
using ConstructionSite.Services.Activity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ConstructionSite.Services.Activity.Infrastructure.Repositories;

public class ActivityRepository(ActivityDbContext dbContext) : IActivityRepository
{
    private readonly ActivityDbContext _dbContext = dbContext;

    public async Task<(IEnumerable<Domain.Entities.Activity>, int, int, int)> GetAllActivitiesAsync(int pageNumber, int pageSize, string? orderBy, string? filter)
    {
        IQueryable<Domain.Entities.Activity> query = _dbContext.Activities.Include(a => a.ActivityType).AsNoTracking();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();
        query = !string.IsNullOrEmpty(orderBy) ? query.OrderBy(orderBy) : query.OrderBy(a => a.Id);

        var activities = await query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

        return (activities, totalCount, activities.Count, (int)Math.Ceiling((double)totalCount / pageSize));
    }

    public async Task<Domain.Entities.Activity?> GetByIdAsync(Ulid id) => await _dbContext.Activities.Include(a => a.ActivityType).FirstOrDefaultAsync(a => a.Id == id);

    public async Task AddAsync(Domain.Entities.Activity activity)
    {
        _dbContext.Activities.Add(activity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Activity activity)
    {
        _dbContext.Activities.Update(activity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ulid id)
    {
        var activity = await GetByIdAsync(id);
        if (activity != null)
        {
            _dbContext.Activities.Remove(activity);
            await _dbContext.SaveChangesAsync();
        }
    }
}