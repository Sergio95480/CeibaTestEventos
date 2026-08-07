using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Domain.Entities;

namespace CeibaTestEventos.Application.Features.Venues.CreateVenue;

public sealed class CreateVenueHandler
{
    private readonly IVenueRepository _venueRepository;

    public CreateVenueHandler(
        IVenueRepository venueRepository)
    {
        _venueRepository = venueRepository;
    }


    public async Task<CreateVenueResult> Handle(
        CreateVenueCommand command,
        CancellationToken cancellationToken)
    {
        var venue = new Venue(
            command.Nombre,
            command.Ciudad,
            command.Capacidad);


        await _venueRepository.AddAsync(
            venue,
            cancellationToken);


        return new CreateVenueResult(
            venue.Id,
            venue.Nombre,
            venue.Ciudad,
            venue.Capacidad);
    }
}