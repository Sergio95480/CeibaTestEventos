using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CeibaTestEventos.Infrastructure.Repositories;

public sealed class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task<Event?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Events
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }


    public async Task<IReadOnlyList<Event>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Events
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }


    public async Task AddAsync(
        Event evento,
        CancellationToken cancellationToken)
    {
        await _context.Events.AddAsync(
            evento,
            cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task<bool> HasVenueConflictAsync(
        Guid venueId,
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken)
    {
        return await _context.Events
            .AnyAsync(
                x =>
                    x.VenueId == venueId &&
                    x.FechaInicio < fechaFin &&
                    x.FechaFin > fechaInicio &&
                    x.Estado != Domain.Enums.EventStatus.Cancelled,
                cancellationToken);
    }


    public async Task UpdateAsync(
        Event evento,
        CancellationToken cancellationToken)
    {
        _context.Events.Update(evento);


        await _context.SaveChangesAsync(
            cancellationToken);
    }
}