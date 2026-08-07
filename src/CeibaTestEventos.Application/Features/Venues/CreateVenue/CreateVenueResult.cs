namespace CeibaTestEventos.Application.Features.Venues.CreateVenue;

public sealed record CreateVenueResult(
    Guid Id,
    string Nombre,
    string Ciudad,
    int Capacidad);