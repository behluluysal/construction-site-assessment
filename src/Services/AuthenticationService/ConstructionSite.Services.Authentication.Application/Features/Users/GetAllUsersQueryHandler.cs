using ConstructionSite.Services.Authentication.Application.DTOs;
using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class GetAllUsersHandler(IUserRepository userRepository, ILogger<GetAllUsersHandler> logger)
    : IRequestHandler<GetAllUsersQuery, Result<UserListResultDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<GetAllUsersHandler> _logger = logger;

    public async Task<Result<UserListResultDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching users. Page: {PageNumber}, Size: {PageSize}, Filter: {Filter}",
            request.PageNumber, request.PageSize, request.Filter ?? "None");

        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            _logger.LogWarning("Invalid pagination parameters. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);
            return Result<UserListResultDto>.FailureResult(["Invalid pagination parameters."]);
        }

        var (users, totalCount, elementsInCurrentPage, totalPages) =
                await _userRepository.GetAllUsersAsync(request.PageNumber, request.PageSize, request.OrderBy, request.Filter);

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
