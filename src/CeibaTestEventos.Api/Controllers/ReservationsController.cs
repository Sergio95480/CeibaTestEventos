using CeibaTestEventos.Application.Features.Reservations.CreateReservation;
using Microsoft.AspNetCore.Mvc;

namespace CeibaTestEventos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReservationsController : ControllerBase
{
    private readonly CreateReservationHandler _handler;


    public ReservationsController(
        CreateReservationHandler handler)
    {
        _handler = handler;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(
            command,
            cancellationToken);


        return CreatedAtAction(
            nameof(Create),
            new { id = result.Id },
            result);
    }
}