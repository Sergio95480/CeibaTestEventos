using CeibaTestEventos.Application.Features.Venues.CreateVenue;
using CeibaTestEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CeibaTestEventos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class VenuesController : ControllerBase
{
    private readonly CreateVenueHandler _handler;
    private readonly IVenueRepository _venueRepository;

    public VenuesController(
        CreateVenueHandler handler,
        IVenueRepository venueRepository)
    {
        _handler = handler;
        _venueRepository = venueRepository;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateVenueCommand command,
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
        var venues = await _venueRepository.GetAllAsync(
            cancellationToken);

        return Ok(venues);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var venue = await _venueRepository.GetByIdAsync(
            id,
            cancellationToken);


        if (venue is null)
        {
            return NotFound();
        }


        return Ok(venue);
    }
}