using ConstructionSite.Services.Authentication.Application.DTOs;
using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class GetWorkersQueryHandler(IUserRepository userRepository, ILogger<GetWorkersQueryHandler> logger)
    : IRequestHandler<GetWorkersQuery, Result<UserListResultDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<GetWorkersQueryHandler> _logger = logger;

    public async Task<Result<UserListResultDto>> Handle(GetWorkersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching users with Worker role");

        string workerRoleFilter = "UserRoles.Any(Role.Name == \"Worker\")";

        string combinedFilter = !string.IsNullOrEmpty(request.Filter)
            ? $"({request.Filter}) and ({workerRoleFilter})"
            : workerRoleFilter;

        var (users, totalCount, elementsInCurrentPage, totalPages) = 
            await _userRepository.GetAllUsersAsync(request.PageNumber, request.PageSize, request.OrderBy, combinedFilter);

        var userDtos = users.Select(user => new UserDto(user.Id,
                                                user.UserName ?? throw new Exception("Username name was null."),
                                                user.Email ?? throw new Exception("Email was null."),
                                                user.Name,
                                                user.Surname)).ToList();
        var result = new UserListResultDto
        {
            Users = userDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = totalPages,
            ElementsInCurrentPage = elementsInCurrentPage
        };

        return Result<UserListResultDto>.SuccessResult(result);
    }
}