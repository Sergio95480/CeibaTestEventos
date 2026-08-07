using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Reservations.CreateReservation;

public sealed record CreateReservationResult(
    Guid Id,
    Guid EventId,
    string CompradorEmail,
    int Cantidad,
    ReservationStatus Estado,
    string? CodigoConfirmacion);