using CeibaTestEventos.Application.Features.Reservations.CancelReservation;
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
    private readonly CancelReservationHandler _cancelHandler;


    public ReservationsController(
        CreateReservationHandler createHandler,
        ConfirmReservationHandler confirmHandler,
        CancelReservationHandler cancelHandler)
    {
        _createHandler = createHandler;
        _confirmHandler = confirmHandler;
        _cancelHandler = cancelHandler;
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


    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CancelReservationCommand(id);

        var result = await _cancelHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }
}