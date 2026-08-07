using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CeibaTestEventos.Infrastructure.Repositories;

public sealed class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task<Reservation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }


    public async Task<IReadOnlyList<Reservation>> GetByEventIdAsync(
        Guid eventId,
        CancellationToken cancellationToken)
    {
        return await _context.Reservations
            .Where(x => x.EventId == eventId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }


    public async Task AddAsync(
        Reservation reservation,
        CancellationToken cancellationToken)
    {
        await _context.Reservations.AddAsync(
            reservation,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task UpdateAsync(
        Reservation reservation,
        CancellationToken cancellationToken)
    {
        _context.Reservations.Update(reservation);

        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task<bool> ExistsConfirmationCodeAsync(
        string codigoConfirmacion,
        CancellationToken cancellationToken)
    {
        return await _context.Reservations
            .AnyAsync(
                x => x.CodigoConfirmacion == codigoConfirmacion,
                cancellationToken);
    }
}