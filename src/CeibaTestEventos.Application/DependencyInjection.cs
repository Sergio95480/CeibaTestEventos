using CeibaTestEventos.Application.Features.Events.CreateEvent;
using CeibaTestEventos.Application.Features.Reservations.CreateReservation;
using CeibaTestEventos.Application.Features.Venues.CreateVenue;
using CeibaTestEventos.Application.Features.Reservations.ConfirmReservation;
using CeibaTestEventos.Application.Features.Reservations.CancelReservation;
using CeibaTestEventos.Application.Features.Events.CompleteEvent;
using CeibaTestEventos.Application.Features.Events.PublishEvent;
using CeibaTestEventos.Application.Features.Events.OccupationReport;

using Microsoft.Extensions.DependencyInjection;

namespace CeibaTestEventos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreateVenueHandler>();

        services.AddScoped<CreateEventHandler>();

        services.AddScoped<CreateReservationHandler>();

        services.AddScoped<ConfirmReservationHandler>();

        services.AddScoped<CancelReservationHandler>();

        services.AddScoped<CompleteEventHandler>();

        services.AddScoped<PublishEventHandler>();

	services.AddScoped<GetOccupationReportHandler>();

        return services;
    }
}