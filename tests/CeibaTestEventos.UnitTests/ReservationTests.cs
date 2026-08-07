using CeibaTestEventos.Domain.Common;
using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Domain.Enums;
using CeibaTestEventos.Domain.ValueObjects;
using Xunit;

namespace CeibaTestEventos.UnitTests;

public class ReservationTests
{
    [Fact]
    public void CrearReserva_EmailValido_DebeCrearReserva()
    {
        var reservation = new Reservation(
            Guid.NewGuid(),
            Email.Create("cliente@test.com"),
            2,
            50000,
            DateTime.UtcNow.AddDays(5),
            DateTime.UtcNow);


        Assert.Equal(
            2,
            reservation.Cantidad);
    }


    [Fact]
    public void CrearReserva_EmailInvalido_DebeLanzarExcepcion()
    {
        Assert.Throws<DomainException>(() =>
            Email.Create("cliente"));
    }


    [Fact]
    public void CrearReserva_CantidadCero_DebeLanzarExcepcion()
    {
        Assert.Throws<DomainException>(() =>
            new Reservation(
                Guid.NewGuid(),
                Email.Create("cliente@test.com"),
                0,
                50000,
                DateTime.UtcNow.AddDays(5),
                DateTime.UtcNow));
    }


    [Fact]
    public void CrearReserva_MenosDe24Horas_MasDe5Entradas_DebeLanzarExcepcion()
    {
        Assert.Throws<DomainException>(() =>
            new Reservation(
                Guid.NewGuid(),
                Email.Create("cliente@test.com"),
                6,
                50000,
                DateTime.UtcNow.AddHours(20),
                DateTime.UtcNow));
    }


    [Fact]
    public void CrearReserva_MenosDe24Horas_5Entradas_DebePermitir()
    {
        var reserva = new Reservation(
            Guid.NewGuid(),
            Email.Create("cliente@test.com"),
            5,
            50000,
            DateTime.UtcNow.AddHours(20),
            DateTime.UtcNow);


        Assert.Equal(
            ReservationStatus.Pending,
            reserva.Estado);
    }


    [Fact]
    public void ConfirmarReserva_GeneraCodigoConfirmacion()
    {
        var reserva = new Reservation(
            Guid.NewGuid(),
            Email.Create("cliente@test.com"),
            2,
            50000,
            DateTime.UtcNow.AddDays(5),
            DateTime.UtcNow);


        reserva.Confirmar();


        Assert.Equal(
            ReservationStatus.Confirmed,
            reserva.Estado);

        Assert.False(
            string.IsNullOrEmpty(reserva.CodigoConfirmacion));
    }
[Fact]
public void CancelarReserva_MasDe48Horas_DebeQuedarCancelada()
{
    var reserva = new Reservation(
        Guid.NewGuid(),
        Email.Create("cliente@test.com"),
        2,
        50000,
        DateTime.UtcNow.AddDays(5),
        DateTime.UtcNow);

    reserva.Confirmar();

    reserva.Cancelar(
        DateTime.UtcNow.AddDays(5),
        DateTime.UtcNow);


    Assert.Equal(
        ReservationStatus.Cancelled,
        reserva.Estado);
}


[Fact]
public void CancelarReserva_MenosDe48Horas_DebeQuedarPerdida()
{
    var reserva = new Reservation(
        Guid.NewGuid(),
        Email.Create("cliente@test.com"),
        2,
        50000,
        DateTime.UtcNow.AddHours(24),
        DateTime.UtcNow);

    reserva.Confirmar();

    reserva.Cancelar(
        DateTime.UtcNow.AddHours(24),
        DateTime.UtcNow);


    Assert.Equal(
        ReservationStatus.Lost,
        reserva.Estado);
}
}