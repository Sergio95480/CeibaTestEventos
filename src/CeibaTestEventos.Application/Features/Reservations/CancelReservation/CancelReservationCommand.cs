namespace CeibaTestEventos.Application.Features.Reservations.CancelReservation;

public sealed record CancelReservationCommand(
    Guid ReservationId);