using CeibaTestEventos.Domain.Entities;

namespace CeibaTestEventos.Application.Interfaces;

public interface IVenueRepository
{
    Task<Venue?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);


    Task<IReadOnlyList<Venue>> GetAllAsync(
        CancellationToken cancellationToken);


    Task AddAsync(
        Venue venue,
        CancellationToken cancellationToken);


    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken);
}