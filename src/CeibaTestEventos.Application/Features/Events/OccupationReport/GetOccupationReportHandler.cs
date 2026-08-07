using CeibaTestEventos.Application.Interfaces;

namespace CeibaTestEventos.Application.Features.Events.OccupationReport;

public sealed class GetOccupationReportHandler
{
    private readonly IEventRepository _eventRepository;


    public GetOccupationReportHandler(
        IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }


    public async Task<EventOccupationReportDto> Handle(
        GetOccupationReportQuery query,
        CancellationToken cancellationToken)
    {
        var evento = await _eventRepository.GetByIdAsync(
            query.EventId,
            cancellationToken);


        if (evento is null)
        {
            throw new Exception(
                "Evento no encontrado.");
        }


        var porcentajeOcupacion =
            evento.Capacidad == 0
            ? 0
            : Math.Round(
                (decimal)evento.EntradasReservadas /
                evento.Capacidad * 100,
                2);


        var ingresos =
            evento.Precio *
            evento.EntradasReservadas;


        return new EventOccupationReportDto(
            evento.Id,
            evento.Nombre,
            evento.EntradasReservadas,
            evento.Capacidad - evento.EntradasReservadas,
            porcentajeOcupacion,
            ingresos,
            evento.Estado.ToString());
    }
}