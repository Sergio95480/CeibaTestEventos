using CeibaTestEventos.Application.Features.Reservations.ConfirmReservation;
using CeibaTestEventos.Application.Features.Reservations.CreateReservation;
using Microsoft.AspNetCore.Mvc;

namespace CeibaTestEventos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReservationsController : ControllerBase
{
    private readonly CreateReservationHandler _createHandler;
    private readonly ConfirmReservationHandler _confirmHandler;


    public ReservationsController(
        CreateReservationHandler createHandler,
        ConfirmReservationHandler confirmHandler)
    {
        _createHandler = createHandler;
        _confirmHandler = confirmHandler;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _createHandler.Handle(
            command,
            cancellationToken);


        return CreatedAtAction(
            nameof(Create),
            new { id = result.Id },
            result);
    }


    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmReservationCommand(id);


        var result = await _confirmHandler.Handle(
            command,
            cancellationToken);


        return Ok(result);
    }
}