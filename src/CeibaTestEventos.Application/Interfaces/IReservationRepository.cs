using CeibaTestEventos.Domain.Entities;

namespace CeibaTestEventos.Application.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);


    Task<IReadOnlyList<Reservation>> GetByEventIdAsync(
        Guid eventId,
        CancellationToken cancellationToken);


    Task AddAsync(
        Reservation reservation,
        CancellationToken cancellationToken);


    Task UpdateAsync(
        Reservation reservation,
        CancellationToken cancellationToken);


    Task<bool> ExistsConfirmationCodeAsync(
        string codigoConfirmacion,
        CancellationToken cancellationToken);
}