using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Domain.Common;
using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Domain.Enums;
using CeibaTestEventos.Domain.ValueObjects;

namespace CeibaTestEventos.Application.Features.Reservations.CreateReservation;

public sealed class CreateReservationHandler
{
    private readonly IEventRepository _eventRepository;
    private readonly IReservationRepository _reservationRepository;


    public CreateReservationHandler(
        IEventRepository eventRepository,
        IReservationRepository reservationRepository)
    {
        _eventRepository = eventRepository;
        _reservationRepository = reservationRepository;
    }


    public async Task<CreateReservationResult> Handle(
        CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        var evento = await _eventRepository.GetByIdAsync(
            command.EventId,
            cancellationToken);


        if (evento is null)
        {
            throw new DomainException(
                "El evento indicado no existe.");
        }


        ValidateReservationWindow(evento);


        ValidateTransactionLimit(
            evento,
            command.Cantidad);


        evento.ReservarEntradas(
            command.Cantidad);


        var reservation = new Reservation(
            evento.Id,
            Email.Create(command.CompradorEmail),
            command.Cantidad,
            evento.Precio,
            evento.FechaInicio,
            DateTime.UtcNow);


        await _eventRepository.UpdateAsync(
            evento,
            cancellationToken);


        await _reservationRepository.AddAsync(
            reservation,
            cancellationToken);


        return new CreateReservationResult(
            reservation.Id,
            reservation.EventId,
            reservation.CompradorEmail.Value,
            reservation.Cantidad,
            reservation.Estado,
            reservation.CodigoConfirmacion);
    }


    private static void ValidateReservationWindow(
        Event evento)
    {
        var minimumReservationTime =
            DateTime.UtcNow.AddHours(1);


        if (evento.FechaInicio <= minimumReservationTime)
        {
            throw new DomainException(
                "No se permiten reservas para eventos que inicien en menos de una hora.");
        }
    }


    private static void ValidateTransactionLimit(
        Event evento,
        int cantidad)
    {
        if (evento.Precio > 100 &&
            cantidad > 10)
        {
            throw new DomainException(
                "Los eventos con precio superior a $100 permiten máximo 10 entradas por transacción.");
        }
    }
}