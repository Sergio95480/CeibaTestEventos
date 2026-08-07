namespace CeibaTestEventos.Application.Events.Queries.GetEventOccupationReport;

public sealed record EventOccupationReportDto(
    Guid EventId,
    string NombreEvento,
    int EntradasVendidas,
    int EntradasDisponibles,
    decimal PorcentajeOcupacion,
    decimal IngresosTotales,
    string Estado);