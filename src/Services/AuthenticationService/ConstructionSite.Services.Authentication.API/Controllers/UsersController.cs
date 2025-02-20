using ConstructionSite.Services.Authentication.Application.Features.Users;
using ConstructionSite.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionSite.Services.Authentication.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? orderBy, [FromQuery] string? filter)
    {
        var result = await mediator.Send(new GetAllUsersQuery(pageNumber, pageSize, orderBy, filter));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.Succeeded)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetUserById), new { id = result.Data }, result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUserById(Ulid id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id.ToString()));
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.Succeeded)
            return BadRequest(result);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await mediator.Send(new DeleteUserCommand(id));
        if (!result.Succeeded)
            return BadRequest(result);
        return NoContent();
    }

    [HttpGet("{userId}/roles")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var roles = await mediator.Send(new GetUserRolesQuery(userId));
        return Ok(roles);
    }

    [HttpPost("{userId}/roles/{roleName}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var result = await mediator.Send(new AssignRoleCommand(userId, roleName));
        if (!result.Succeeded)
            return BadRequest(result);
        return NoContent();
    }

    [HttpDelete("{userId}/roles/{roleName}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> RevokeRole(string userId, string roleName)
    {
        var result = await mediator.Send(new RevokeRoleCommand(userId, roleName));
        if (!result.Succeeded)
            return BadRequest(result);
        return NoContent();
    }

    [HttpGet("workers")]
    [Authorize(Policy = Permissions.Users.ViewWorkers)]
    public async Task<IActionResult> GetWorkers([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? orderBy, [FromQuery] string? filter)
    {
        var result = await mediator.Send(new GetWorkersQuery(pageNumber, pageSize, orderBy, filter));
        return Ok(result);
    }
}