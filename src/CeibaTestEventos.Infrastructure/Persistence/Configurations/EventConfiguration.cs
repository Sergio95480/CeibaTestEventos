using CeibaTestEventos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CeibaTestEventos.Infrastructure.Persistence.Configurations;

public sealed class EventConfiguration :
    IEntityTypeConfiguration<Event>
{
    public void Configure(
        EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.Nombre)
            .HasMaxLength(200)
            .IsRequired();


        builder.Property(x => x.Precio)
            .HasPrecision(10, 2)
            .IsRequired();


        builder.Property(x => x.Capacidad)
            .IsRequired();


        builder.Property(x => x.EntradasReservadas)
            .IsRequired();


        builder.Property(x => x.TipoEvento)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();


        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();


        builder.Property(x => x.FechaInicio)
            .IsRequired();


        builder.Property(x => x.FechaFin)
            .IsRequired();


        builder.HasOne<Venue>()
            .WithMany()
            .HasForeignKey(x => x.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}