using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Events.GetEvents;

public sealed class EventFilterRequest
{
    public EventType? TipoEvento { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public Guid? VenueId { get; set; }

    public EventStatus? Estado { get; set; }

    public string? Titulo { get; set; }
}