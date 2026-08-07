using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.DTOs;

public sealed record ReservationDto(
    Guid Id,
    Guid EventId,
    string CompradorEmail,
    int Cantidad,
    ReservationStatus Estado,
    string? CodigoConfirmacion);