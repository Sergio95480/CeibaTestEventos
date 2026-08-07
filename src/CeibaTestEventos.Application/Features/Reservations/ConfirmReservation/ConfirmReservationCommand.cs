namespace CeibaTestEventos.Application.Features.Reservations.ConfirmReservation;

public sealed record ConfirmReservationCommand(
    Guid ReservationId);