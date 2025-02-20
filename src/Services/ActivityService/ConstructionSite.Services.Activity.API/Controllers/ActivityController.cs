using ConstructionSite.Services.Activity.Application.Features.Activities;
using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionSite.Services.Activity.API.Controllers;

[ApiController]
[Route("api/activities")]
public class ActivityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Authorize(Policy = Permissions.Activities.Create)]
    public async Task<IActionResult> CreateActivity(CreateActivityCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Succeeded)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetActivityById), new { id = result.Data }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.Activities.Edit)]
    public async Task<IActionResult> UpdateActivity(Ulid id, UpdateActivityCommand command)
    {
        if (id != command.Id) return BadRequest("Mismatched ID.");
        var result = await _mediator.Send(command);
        if (!result.Succeeded)
            return BadRequest(result);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Activities.Delete)]
    public async Task<IActionResult> DeleteActivity(Ulid id)
    {
        var result = await _mediator.Send(new DeleteActivityCommand(id));
        if (!result.Succeeded)
            return BadRequest(result);
        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.Activities.View)]
    public async Task<IActionResult> GetActivityById(Ulid id)
    {
        var result = await _mediator.Send(new GetActivityByIdQuery(id));
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Activities.View)]
    public async Task<IActionResult> GetAllActivities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? orderBy = null, [FromQuery] string? filter = null)
    {
        var query = new GetAllActivitiesQuery(pageNumber, pageSize, orderBy, filter);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}