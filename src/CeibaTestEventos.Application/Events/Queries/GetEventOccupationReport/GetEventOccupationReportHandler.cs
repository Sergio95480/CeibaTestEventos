using CeibaTestEventos.Application.Interfaces;

namespace CeibaTestEventos.Application.Events.Queries.GetEventOccupationReport;

public sealed class GetEventOccupationReportHandler
{
    private readonly IEventRepository _repository;

    public GetEventOccupationReportHandler(
        IEventRepository repository)
    {
        _repository = repository;
    }


    public async Task<EventOccupationReportDto> Handle(
        GetEventOccupationReportQuery query,
        CancellationToken cancellationToken)
    {
        var evento = await _repository.GetByIdAsync(
            query.EventId,
            cancellationToken);


        if (evento is null)
        {
            throw new Exception(
                "Evento no encontrado.");
        }


        var porcentaje =
            evento.Capacidad == 0
                ? 0
                : (decimal)evento.EntradasReservadas /
                  evento.Capacidad * 100;


        var ingresos =
            evento.Precio *
            evento.EntradasReservadas;


        return new EventOccupationReportDto(
            evento.Id,
            evento.Nombre,
            evento.EntradasReservadas,
            evento.Capacidad - evento.EntradasReservadas,
            Math.Round(porcentaje, 2),
            ingresos,
            evento.Estado.ToString());
    }
}