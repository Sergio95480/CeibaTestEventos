using CeibaTestEventos.Application.Interfaces;

namespace CeibaTestEventos.Application.Features.Events.PublishEvent;

public sealed class PublishEventHandler
{
    private readonly IEventRepository _eventRepository;


    public PublishEventHandler(
        IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }


    public async Task<PublishEventResult> Handle(
        PublishEventCommand command,
        CancellationToken cancellationToken)
    {
        var evento =
            await _eventRepository.GetByIdAsync(
                command.EventId,
                cancellationToken);


        if (evento is null)
        {
            throw new InvalidOperationException(
                "El evento indicado no existe.");
        }


        evento.Publicar();


        await _eventRepository.UpdateAsync(
            evento,
            cancellationToken);


        return new PublishEventResult(
            evento.Id,
            evento.Nombre,
            evento.Estado);
    }
}