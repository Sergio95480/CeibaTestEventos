using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Domain.Common;
using CeibaTestEventos.Domain.Entities;

namespace CeibaTestEventos.Application.Features.Events.CreateEvent;

public sealed class CreateEventHandler
{
    private readonly IVenueRepository _venueRepository;
    private readonly IEventRepository _eventRepository;


    public CreateEventHandler(
        IVenueRepository venueRepository,
        IEventRepository eventRepository)
    {
        _venueRepository = venueRepository;
        _eventRepository = eventRepository;
    }


    public async Task<CreateEventResult> Handle(
        CreateEventCommand command,
        CancellationToken cancellationToken)
    {
        var venue = await _venueRepository.GetByIdAsync(
            command.VenueId,
            cancellationToken);


        if (venue is null)
        {
            throw new DomainException(
                "El venue indicado no existe.");
        }


        if (command.Capacidad > venue.Capacidad)
        {
            throw new DomainException(
                "La capacidad del evento supera la capacidad del venue.");
        }


        var fechaInicioUtc = DateTime.SpecifyKind(
            command.FechaInicio,
            DateTimeKind.Utc);


        var fechaFinUtc = DateTime.SpecifyKind(
            command.FechaFin,
            DateTimeKind.Utc);


        var hasConflict = await _eventRepository.HasVenueConflictAsync(
            command.VenueId,
            fechaInicioUtc,
            fechaFinUtc,
            cancellationToken);


        if (hasConflict)
        {
            throw new DomainException(
                "El venue ya tiene un evento programado en ese horario.");
        }


        var evento = new Event(
            command.VenueId,
            command.TipoEvento,
            command.Nombre,
            fechaInicioUtc,
            fechaFinUtc,
            command.Precio,
            command.Capacidad);


        await _eventRepository.AddAsync(
            evento,
            cancellationToken);


        return new CreateEventResult(
            evento.Id,
            evento.VenueId,
            evento.Nombre,
            evento.TipoEvento,
            evento.FechaInicio,
            evento.FechaFin,
            evento.Precio,
            evento.Capacidad,
            evento.Estado);
    }
}