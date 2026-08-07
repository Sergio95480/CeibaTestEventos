using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Infrastructure.Persistence;
using CeibaTestEventos.Application.Features.Events.GetEvents;
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

public async Task<IReadOnlyList<Event>> SearchAsync(
    EventFilterRequest filter,
    CancellationToken cancellationToken)
{
    var query =
        _context.Events.AsQueryable();


    if(filter.TipoEvento.HasValue)
    {
        query = query.Where(x =>
            x.TipoEvento == filter.TipoEvento);
    }


    if(filter.FechaDesde.HasValue)
    {
        query = query.Where(x =>
            x.FechaInicio >= filter.FechaDesde);
    }


    if(filter.FechaHasta.HasValue)
    {
        query = query.Where(x =>
            x.FechaInicio <= filter.FechaHasta);
    }


    if(filter.VenueId.HasValue)
    {
        query = query.Where(x =>
            x.VenueId == filter.VenueId);
    }


    if(filter.Estado.HasValue)
    {
        query = query.Where(x =>
            x.Estado == filter.Estado);
    }


    if(!string.IsNullOrWhiteSpace(filter.Titulo))
    {
        var titulo =
            filter.Titulo.ToLower();


        query = query.Where(x =>
            x.Nombre
            .ToLower()
            .Contains(titulo));
    }


    return await query
        .OrderBy(x => x.FechaInicio)
        .ToListAsync(cancellationToken);
}
}