using CeibaTestEventos.Domain.Common;
using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Domain.Entities;

public sealed class Event : Entity
{
    public Guid VenueId { get; private set; }

    public EventType TipoEvento { get; private set; }

    public string Nombre { get; private set; }

    public DateTime FechaInicio { get; private set; }

    public DateTime FechaFin { get; private set; }

    public decimal Precio { get; private set; }

    public int Capacidad { get; private set; }

    public int EntradasReservadas { get; private set; }

    public EventStatus Estado { get; private set; }


    private Event()
    {
        Nombre = string.Empty;
    }


    public Event(
        Guid venueId,
        EventType tipoEvento,
        string nombre,
        DateTime fechaInicio,
        DateTime fechaFin,
        decimal precio,
        int capacidad)
    {
        Validate(
            venueId,
            nombre,
            fechaInicio,
            fechaFin,
            precio,
            capacidad);

        VenueId = venueId;
        TipoEvento = tipoEvento;
        Nombre = nombre.Trim();
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        Precio = precio;
        Capacidad = capacidad;
        EntradasReservadas = 0;
        Estado = EventStatus.Draft;
    }


    public void Publicar()
    {
        if (Estado != EventStatus.Draft)
        {
            throw new DomainException(
                "Solo un evento en borrador puede publicarse.");
        }

        Estado = EventStatus.Published;
    }


    public void Cancelar()
    {
        if (Estado == EventStatus.Completed)
        {
            throw new DomainException(
                "Un evento completado no puede cancelarse.");
        }

        Estado = EventStatus.Cancelled;
    }


    public void Completar()
    {
        if (Estado != EventStatus.Published)
        {
            throw new DomainException(
                "Solo un evento publicado puede completarse.");
        }

        Estado = EventStatus.Completed;
    }


    public void ReservarEntradas(int cantidad)
    {
        if (cantidad <= 0)
        {
            throw new DomainException(
                "La cantidad debe ser mayor que cero.");
        }

        if (EntradasReservadas + cantidad > Capacidad)
        {
            throw new DomainException(
                "No existen suficientes entradas disponibles.");
        }

        EntradasReservadas += cantidad;
    }


    public void LiberarEntradas(int cantidad)
    {
        if (cantidad <= 0)
        {
            throw new DomainException(
                "La cantidad debe ser mayor que cero.");
        }

        if (cantidad > EntradasReservadas)
        {
            throw new DomainException(
                "No se pueden liberar más entradas de las reservadas.");
        }

        EntradasReservadas -= cantidad;
    }


    private static void Validate(
        Guid venueId,
        string nombre,
        DateTime fechaInicio,
        DateTime fechaFin,
        decimal precio,
        int capacidad)
    {
        if (venueId == Guid.Empty)
        {
            throw new DomainException(
                "El venue asociado es obligatorio.");
        }


        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new DomainException(
                "El nombre del evento es obligatorio.");
        }


        if (fechaFin <= fechaInicio)
        {
            throw new DomainException(
                "La fecha final debe ser mayor que la fecha inicial.");
        }


        if (precio < 0)
        {
            throw new DomainException(
                "El precio no puede ser negativo.");
        }


        if (capacidad <= 0)
        {
            throw new DomainException(
                "La capacidad debe ser mayor que cero.");
        }


        ValidateWeekendSchedule(fechaInicio);
    }


    private static void ValidateWeekendSchedule(
        DateTime fechaInicio)
    {
        var isWeekend =
            fechaInicio.DayOfWeek == DayOfWeek.Saturday ||
            fechaInicio.DayOfWeek == DayOfWeek.Sunday;


        if (isWeekend &&
            fechaInicio.TimeOfDay > new TimeSpan(22, 0, 0))
        {
            throw new DomainException(
                "Los eventos de fin de semana no pueden iniciar después de las 22:00.");
        }
    }
}