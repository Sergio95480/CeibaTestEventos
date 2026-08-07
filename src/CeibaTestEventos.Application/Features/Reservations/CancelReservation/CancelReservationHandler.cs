using CeibaTestEventos.Application.Interfaces;

namespace CeibaTestEventos.Application.Features.Reservations.CancelReservation;

public sealed class CancelReservationHandler
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventRepository _eventRepository;


    public CancelReservationHandler(
        IReservationRepository reservationRepository,
        IEventRepository eventRepository)
    {
        _reservationRepository = reservationRepository;
        _eventRepository = eventRepository;
    }


    public async Task<CancelReservationResult> Handle(
        CancelReservationCommand command,
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


        var evento =
            await _eventRepository.GetByIdAsync(
                reservation.EventId,
                cancellationToken);


        if (evento is null)
        {
            throw new InvalidOperationException(
                "El evento asociado no existe.");
        }


        reservation.Cancelar(
            evento.FechaInicio,
            DateTime.UtcNow);


        await _reservationRepository.UpdateAsync(
            reservation,
            cancellationToken);


        return new CancelReservationResult(
            reservation.Id,
            reservation.EventId,
            reservation.CompradorEmail.Value,
            reservation.Cantidad,
            reservation.Estado,
            reservation.CodigoConfirmacion);
    }
}