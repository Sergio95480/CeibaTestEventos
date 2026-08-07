using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.DTOs;

public sealed record EventDto(
    Guid Id,
    Guid VenueId,
    EventType TipoEvento,
    string Nombre,
    DateTime FechaInicio,
    DateTime FechaFin,
    decimal Precio,
    int Capacidad,
    int EntradasReservadas,
    EventStatus Estado);