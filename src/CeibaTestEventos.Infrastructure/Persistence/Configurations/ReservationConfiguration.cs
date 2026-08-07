using CeibaTestEventos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CeibaTestEventos.Infrastructure.Persistence.Configurations;

public sealed class ReservationConfiguration :
    IEntityTypeConfiguration<Reservation>
{
    public void Configure(
        EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.Cantidad)
            .IsRequired();


        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();


        builder.Property(x => x.CodigoConfirmacion)
            .HasMaxLength(100);


        builder.OwnsOne(
            x => x.CompradorEmail,
            email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("CompradorEmail")
                    .HasMaxLength(150)
                    .IsRequired();
            });


        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}