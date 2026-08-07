using CeibaTestEventos.Application.Interfaces;
using CeibaTestEventos.Infrastructure.Persistence;
using CeibaTestEventos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CeibaTestEventos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"));
        });


        services.AddScoped<IVenueRepository, VenueRepository>();

        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<IReservationRepository, ReservationRepository>();


        return services;
    }
}