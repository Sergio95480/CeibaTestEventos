using CeibaTestEventos.Domain.Enums;

namespace CeibaTestEventos.Application.Features.Events.PublishEvent;

public sealed record PublishEventResult(
    Guid Id,
    string Nombre,
    EventStatus Estado);