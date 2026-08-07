using CeibaTestEventos.Application.Interfaces;

namespace CeibaTestEventos.Application.Features.Reservations.ConfirmReservation;

public sealed class ConfirmReservationHandler
{
    private readonly IReservationRepository _reservationRepository;


    public ConfirmReservationHandler(
        IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }


    public async Task<ConfirmReservationResult> Handle(
        ConfirmReservationCommand command,
        CancellationToken cancellationToken)
    {
        var reservation =
            await _reservationRepository.GetByIdAsync(
                command.ReservationId,
                cancellationToken);


        if (reservation is null)
        {
            throw new InvalidOperationException(
                "La reserva indicada no existe.");
        }


        reservation.Confirmar();


        await _reservationRepository.UpdateAsync(
            reservation,
            cancellationToken);


        return new ConfirmReservationResult(
            reservation.Id,
            reservation.EventId,
            reservation.CompradorEmail.Value,
            reservation.Cantidad,
            reservation.Estado,
            reservation.CodigoConfirmacion);
    }
}