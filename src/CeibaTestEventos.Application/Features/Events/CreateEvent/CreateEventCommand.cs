using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Events.CreateEvent;

public sealed record CreateEventCommand(
    Guid VenueId,
    string Nombre,
    EventType TipoEvento,
    DateTime FechaInicio,
    DateTime FechaFin,
    decimal Precio,
    int Capacidad);