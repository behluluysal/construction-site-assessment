using ConstructionSite.Services.Authentication.Application.Features.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionSite.Services.Authentication.API.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await mediator.Send(new GetAllRolesQuery());
        return Ok(roles);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetRoleById(Ulid id)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id));
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var result = await mediator.Send(new CreateRoleCommand(roleName));
        if (!result.Succeeded)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetRoleById), new { id = result.Data }, result);
    }

    [HttpDelete("{roleName}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var result = await mediator.Send(new DeleteRoleCommand(roleName));
        if (!result.Succeeded)
            return BadRequest(result);

        return NoContent();
    }

    [HttpGet("{roleName}/claims")]
    public async Task<IActionResult> GetRoleClaims(string roleName)
    {
        var claims = await mediator.Send(new GetRoleClaimsQuery(roleName));
        return Ok(claims);
    }

    [HttpPost("{roleName}/claims/{claimName}")]
    public async Task<IActionResult> AssignClaim(string roleName, string claimName)
    {
        var result = await mediator.Send(new AssignClaimCommand(roleName, claimName));
        if (!result.Succeeded)
            return BadRequest(result);

        return NoContent();
    }

    [HttpDelete("{roleName}/claims/{claimName}")]
    public async Task<IActionResult> RevokeClaim(string roleName, string claimName)
    {
        var result = await mediator.Send(new RevokeClaimCommand(roleName, claimName));
        if (!result.Succeeded)
            return BadRequest(result);

        return NoContent();
    }
}
