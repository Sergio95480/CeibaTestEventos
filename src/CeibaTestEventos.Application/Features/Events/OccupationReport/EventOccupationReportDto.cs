namespace CeibaTestEventos.Application.Features.Events.OccupationReport;

public sealed record EventOccupationReportDto(
    Guid EventId,
    string NombreEvento,
    int EntradasVendidas,
    int EntradasDisponibles,
    decimal PorcentajeOcupacion,
    decimal IngresosTotales,
    string Estado);