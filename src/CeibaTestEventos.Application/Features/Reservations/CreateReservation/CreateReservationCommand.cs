namespace CeibaTestEventos.Application.Features.Reservations.CreateReservation;

public sealed record CreateReservationCommand(
    Guid EventId,
    string CompradorEmail,
    int Cantidad);