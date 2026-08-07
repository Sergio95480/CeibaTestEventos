using CeibaTestEventos.Application.Features.Events.GetEvents;
using CeibaTestEventos.Domain.Entities;

namespace CeibaTestEventos.Application.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);


    Task<IReadOnlyList<Event>> GetAllAsync(
        CancellationToken cancellationToken);


    Task AddAsync(
        Event evento,
        CancellationToken cancellationToken);


    Task<bool> HasVenueConflictAsync(
        Guid venueId,
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken);


    Task UpdateAsync(
        Event evento,
        CancellationToken cancellationToken);


    Task<IReadOnlyList<Event>> SearchAsync(
        EventFilterRequest filter,
        CancellationToken cancellationToken);
}