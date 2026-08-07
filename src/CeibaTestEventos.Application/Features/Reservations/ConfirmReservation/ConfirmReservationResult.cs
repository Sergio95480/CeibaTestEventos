using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Reservations.ConfirmReservation;

public sealed record ConfirmReservationResult(
    Guid Id,
    Guid EventId,
    string CompradorEmail,
    int Cantidad,
    ReservationStatus Estado,
    string? CodigoConfirmacion);