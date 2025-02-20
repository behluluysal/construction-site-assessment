using ConstructionSite.Services.Activity.Application.Features.ActivityTypes;
using ConstructionSite.Services.Activity.Domain.Common;
using ConstructionSite.Services.Activity.Domain.Entities;
using ConstructionSite.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionSite.Services.Activity.API.Controllers;

[ApiController]
[Route("api/activity-types")]
public class ActivityTypeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Authorize(Policy = Permissions.ActivityTypes.Create)]
    public async Task<IActionResult> CreateActivityType([FromBody] CreateActivityTypeCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Succeeded)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetActivityTypeById), new { id = result.Data }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.ActivityTypes.Edit)]
    public async Task<IActionResult> UpdateActivityType(Ulid id, UpdateActivityTypeCommand command)
    {
        if (id != command.Id) return BadRequest("Mismatched ID.");

        var result = await _mediator.Send(command);

        if (!result.Succeeded)
            return BadRequest(result);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.ActivityTypes.Delete)]
    public async Task<IActionResult> DeleteActivityType(Ulid id)
    {
        var result = await _mediator.Send(new DeleteActivityTypeCommand(id));
        if (!result.Succeeded)
            return BadRequest(result);

        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.ActivityTypes.View)]
    public async Task<IActionResult> GetActivityTypeById(Ulid id)
    {
        var result = await _mediator.Send(new GetActivityTypeByIdQuery(id));
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = Permissions.ActivityTypes.View)]
    public async Task<IActionResult> GetAllActivityTypes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? orderBy = null, [FromQuery] string? filter = null)
    {
        var query = new GetAllActivityTypesQuery(pageNumber, pageSize, orderBy, filter);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("worker")]
    [Authorize(Policy = Permissions.ActivityTypes.ViewForWorker)]
    public async Task<IActionResult> GetAllActivityTypesForWorker([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? orderBy = null, [FromQuery] string? filter = null)
    {
        var query = new GetAllActivityTypesQuery(pageNumber, pageSize, orderBy, filter);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
