using CeibaTestEventos.Domain.Common;
using CeibaTestEventos.Domain.Enums;
using CeibaTestEventos.Domain.ValueObjects;

namespace CeibaTestEventos.Domain.Entities;

public sealed class Reservation : Entity
{
    public Guid EventId { get; private set; }

    public Email CompradorEmail { get; private set; }

    public int Cantidad { get; private set; }

    public ReservationStatus Estado { get; private set; }

    public string? CodigoConfirmacion { get; private set; }


    private Reservation()
    {
        CompradorEmail = null!;
    }


    public Reservation(
        Guid eventId,
        Email compradorEmail,
        int cantidad,
        decimal precioEvento,
        DateTime fechaInicioEvento,
        DateTime fechaActual)
    {
        ValidateCreation(
            eventId,
            compradorEmail,
            cantidad,
            precioEvento,
            fechaInicioEvento,
            fechaActual);

        EventId = eventId;
        CompradorEmail = compradorEmail;
        Cantidad = cantidad;
        Estado = ReservationStatus.Pending;
    }


    public void Confirmar()
    {
        if (Estado != ReservationStatus.Pending)
        {
            throw new DomainException(
                "Solo una reserva pendiente puede confirmarse.");
        }

        Estado = ReservationStatus.Confirmed;
        CodigoConfirmacion = GenerateConfirmationCode();
    }


    public void Cancelar(
        DateTime fechaInicioEvento,
        DateTime fechaActual)
    {
        if (Estado != ReservationStatus.Confirmed)
        {
            throw new DomainException(
                "Solo una reserva confirmada puede cancelarse.");
        }


        var diferencia = fechaInicioEvento - fechaActual;


        Estado = diferencia.TotalHours < 48
            ? ReservationStatus.Lost
            : ReservationStatus.Cancelled;
    }


    private static void ValidateCreation(
        Guid eventId,
        Email compradorEmail,
        int cantidad,
        decimal precioEvento,
        DateTime fechaInicioEvento,
        DateTime fechaActual)
    {
        if (eventId == Guid.Empty)
        {
            throw new DomainException(
                "El evento asociado es obligatorio.");
        }


        if (compradorEmail is null)
        {
            throw new DomainException(
                "El comprador es obligatorio.");
        }


        if (cantidad <= 0)
        {
            throw new DomainException(
                "La cantidad debe ser mayor que cero.");
        }


        if (fechaInicioEvento - fechaActual < TimeSpan.FromHours(1))
        {
            throw new DomainException(
                "No se permiten reservas una hora antes del evento.");
        }


        if (precioEvento > 100 && cantidad > 10)
        {
            throw new DomainException(
                "Los eventos superiores a $100 permiten máximo 10 entradas por transacción.");
        }
    }


    private static string GenerateConfirmationCode()
    {
        return Guid.NewGuid()
            .ToString("N")
            .Substring(0, 8)
            .ToUpperInvariant();
    }
}