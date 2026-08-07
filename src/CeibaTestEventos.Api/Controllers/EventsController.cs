using CeibaTestEventos.Application.Features.Events.CompleteEvent;
using CeibaTestEventos.Application.Features.Events.CreateEvent;
using CeibaTestEventos.Application.Features.Events.PublishEvent;
using CeibaTestEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CeibaTestEventos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EventsController : ControllerBase
{
    private readonly CreateEventHandler _createHandler;
    private readonly PublishEventHandler _publishHandler;
    private readonly CompleteEventHandler _completeHandler;
    private readonly IEventRepository _eventRepository;

    public EventsController(
        CreateEventHandler createHandler,
        PublishEventHandler publishHandler,
        CompleteEventHandler completeHandler,
        IEventRepository eventRepository)
    {
        _createHandler = createHandler;
        _publishHandler = publishHandler;
        _completeHandler = completeHandler;
        _eventRepository = eventRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEventCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _createHandler.Handle(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _publishHandler.Handle(
            new PublishEventCommand(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _completeHandler.Handle(
            new CompleteEventCommand(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(
            cancellationToken);

        return Ok(events);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var evento = await _eventRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (evento is null)
        {
            return NotFound();
        }

        return Ok(evento);
    }
}