using ConstructionSite.Services.Authentication.Domain.Contracts;
using ConstructionSite.Services.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ConstructionSite.Services.Authentication.Infrastructure.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<(IEnumerable<ApplicationUser>, int, int, int)> GetAllUsersAsync(int pageNumber, int pageSize, string? orderBy, string? filter)
    {
        IQueryable<ApplicationUser> query = _userManager.Users.Include(x => x.UserRoles).ThenInclude(ur => ur.Role).AsNoTracking();


        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(filter);
        }

        var totalUsers = await query.CountAsync();
        query = !string.IsNullOrEmpty(orderBy) ? query.OrderBy(orderBy) : query.OrderBy(u => u.Id);

        var users = await query.Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return (users, totalUsers, users.Count, (int)Math.Ceiling((double)totalUsers / pageSize));
    }
}
