using CeibaTestEventos.Application.Features.Events.CreateEvent;
using CeibaTestEventos.Application.Features.Reservations.CreateReservation;
using CeibaTestEventos.Application.Features.Venues.CreateVenue;
using CeibaTestEventos.Application.Features.Reservations.ConfirmReservation;
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

        return services;
    }
}