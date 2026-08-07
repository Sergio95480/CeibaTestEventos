using CeibaTestEventos.Domain.Common;
using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Domain.Enums;
using CeibaTestEventos.Domain.ValueObjects;
using Xunit;

namespace CeibaTestEventos.UnitTests;

public class ReservationTests
{
    [Fact]
    public void CrearReserva_EventoIniciaEnMenosDeUnaHora_LanzaExcepcion()
    {
        var fechaActual = DateTime.UtcNow;

        var fechaInicioEvento = fechaActual.AddMinutes(30);


        var exception = Assert.Throws<DomainException>(() =>
            new Reservation(
                Guid.NewGuid(),
                Email.Create("cliente@test.com"),
                2,
                50,
                fechaInicioEvento,
                fechaActual));


        Assert.Equal(
            "No se permiten reservas una hora antes del evento.",
            exception.Message);
    }


    [Fact]
    public void CancelarReserva_ConMenosDe48Horas_CambiaEstadoALost()
    {
        var fechaActual = DateTime.UtcNow;


        var reserva = new Reservation(
            Guid.NewGuid(),
            Email.Create("cliente@test.com"),
            2,
            50,
            fechaActual.AddHours(24),
            fechaActual);


        reserva.Confirmar();


        reserva.Cancelar(
            fechaActual.AddHours(24),
            fechaActual);


        Assert.Equal(
            ReservationStatus.Lost,
            reserva.Estado);
    }


    [Fact]
    public void CancelarReserva_ConMasDe48Horas_CambiaEstadoACancelled()
    {
        var fechaActual = DateTime.UtcNow;


        var reserva = new Reservation(
            Guid.NewGuid(),
            Email.Create("cliente@test.com"),
            2,
            50,
            fechaActual.AddDays(5),
            fechaActual);


        reserva.Confirmar();


        reserva.Cancelar(
            fechaActual.AddDays(5),
            fechaActual);


        Assert.Equal(
            ReservationStatus.Cancelled,
            reserva.Estado);
    }
}