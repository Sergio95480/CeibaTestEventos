using CeibaTestEventos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CeibaTestEventos.Infrastructure.Persistence.Configurations;

public sealed class VenueConfiguration :
    IEntityTypeConfiguration<Venue>
{
    public void Configure(
        EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("venues");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.Nombre)
            .HasMaxLength(150)
            .IsRequired();


        builder.Property(x => x.Ciudad)
            .HasMaxLength(100)
            .IsRequired();


        builder.Property(x => x.Capacidad)
            .IsRequired();
    }
}