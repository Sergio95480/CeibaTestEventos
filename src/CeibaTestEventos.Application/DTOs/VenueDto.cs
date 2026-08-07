namespace CeibaTestEventos.Application.DTOs;

public sealed record VenueDto(
    Guid Id,
    string Nombre,
    string Ciudad,
    int Capacidad);