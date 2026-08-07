using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CeibaTestEventos.Infrastructure.Repositories;

public sealed class VenueRepository : IVenueRepository
{
    private readonly AppDbContext _context;


    public VenueRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task<Venue?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Venues
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }


    public async Task<IReadOnlyList<Venue>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Venues
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }


    public async Task AddAsync(
        Venue venue,
        CancellationToken cancellationToken)
    {
        await _context.Venues.AddAsync(
            venue,
            cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Venues
            .AnyAsync(
                x => x.Id == id,
                cancellationToken);
    }
}