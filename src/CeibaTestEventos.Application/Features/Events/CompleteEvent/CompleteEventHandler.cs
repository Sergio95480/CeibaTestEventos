using CeibaTestEventos.Application.Interfaces;

namespace CeibaTestEventos.Application.Features.Events.CompleteEvent;

public sealed class CompleteEventHandler
{
    private readonly IEventRepository _eventRepository;


    public CompleteEventHandler(
        IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }


    public async Task<CompleteEventResult> Handle(
        CompleteEventCommand command,
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


        if (evento.FechaFin > DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "El evento todavía no ha finalizado.");
        }


        evento.Completar();


        await _eventRepository.UpdateAsync(
            evento,
            cancellationToken);


        return new CompleteEventResult(
            evento.Id,
            evento.Nombre,
            evento.Estado);
    }
}