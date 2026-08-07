using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Reservations.CancelReservation;

public sealed record CancelReservationResult(
    Guid Id,
    Guid EventId,
    string CompradorEmail,
    int Cantidad,
    ReservationStatus Estado,
    string? CodigoConfirmacion);