using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Events.CompleteEvent;

public sealed record CompleteEventResult(
    Guid Id,
    string Nombre,
    EventStatus Estado);