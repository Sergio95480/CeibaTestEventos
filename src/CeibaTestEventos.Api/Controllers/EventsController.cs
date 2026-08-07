using CeibaTestEventos.Application.Features.Events.CreateEvent;
using CeibaTestEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CeibaTestEventos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EventsController : ControllerBase
{
    private readonly CreateEventHandler _handler;
    private readonly IEventRepository _eventRepository;


    public EventsController(
        CreateEventHandler handler,
        IEventRepository eventRepository)
    {
        _handler = handler;
        _eventRepository = eventRepository;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEventCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(
            command,
            cancellationToken);


        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
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