using App.Commands;
using App.Dto;
using App.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new event", Description = "Provide event details to create a new event.")]
    [SwaggerResponse(201, "Event created successfully")]
    [SwaggerResponse(400, "Invalid input")]
    public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
    {
        var createdEvent = await _mediator.Send(new CreateEventCommand(eventDto));
        
        return CreatedAtAction(nameof(GetEventById), createdEvent);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieves an event by ID", Description = "Fetch details of a specific event.")]
    [SwaggerResponse(200, "Event found")]
    [SwaggerResponse(404, "Event not found")]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var calendarEvent = await _mediator.Send(new GetEventByIdQuery(id));
        
        if (calendarEvent == null) 
            return NotFound();
        
        return Ok(calendarEvent);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all events", Description = "Fetch all events with optional filters.")]
    [SwaggerResponse(200, "Events retrieved successfully")]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _mediator.Send(new GetAllEventsQuery());
        
        return Ok(events);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Updates an existing event", Description = "Provide updated details for an event.")]
    [SwaggerResponse(200, "Event updated successfully")]
    [SwaggerResponse(404, "Event not found")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventDto eventDto)
    {
        var updatedEvent = await _mediator.Send(new UpdateEventCommand(id, eventDto));
        
        return Ok(updatedEvent);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes an event", Description = "Remove an event from the system.")]
    [SwaggerResponse(204, "Event deleted successfully")]
    [SwaggerResponse(404, "Event not found")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        await _mediator.Send(new DeleteEventCommand(id));
        
        return NoContent();
    }
}