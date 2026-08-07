namespace CeibaTestEventos.Application.Features.Venues.CreateVenue;

public sealed record CreateVenueCommand(
    string Nombre,
    string Ciudad,
    int Capacidad);