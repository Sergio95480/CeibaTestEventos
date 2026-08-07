using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Events.CreateEvent;

public sealed record CreateEventResult(
    Guid Id,
    Guid VenueId,
    string Nombre,
    EventType TipoEvento,
    DateTime FechaInicio,
    DateTime FechaFin,
    decimal Precio,
    int Capacidad,
    EventStatus Estado);