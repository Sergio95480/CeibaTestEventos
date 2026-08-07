using CeibaTestEventos.Domain.Entities;
using CeibaTestEventos.Domain.Enums;
using Xunit;
using CeibaTestEventos.Domain.Common;

namespace CeibaTestEventos.UnitTests;

public class EventTests
{
    [Fact]
    public void Publicar_EventoDraft_CambiaEstadoAPublished()
    {
        var evento = new Event(
            Guid.NewGuid(),
            EventType.Concierto,
            "Concierto prueba",
            DateTime.UtcNow.AddDays(10),
            DateTime.UtcNow.AddDays(10).AddHours(3),
            50,
            100);

        evento.Publicar();

        Assert.Equal(
            EventStatus.Published,
            evento.Estado);
    }


    [Fact]
    public void ReservarEntradas_CantidadMayorCapacidad_LanzaExcepcion()
    {
        var evento = new Event(
            Guid.NewGuid(),
            EventType.Concierto,
            "Concierto prueba",
            DateTime.UtcNow.AddDays(10),
            DateTime.UtcNow.AddDays(10).AddHours(3),
            50,
            100);


        var exception = Assert.Throws<DomainException>(() =>
            evento.ReservarEntradas(101));


        Assert.Equal(
            "No existen suficientes entradas disponibles.",
            exception.Message);
    }


    [Fact]
    public void CrearEvento_SabadoDespuesDeLas22_LanzaExcepcion()
    {
        var fechaInicio =
            new DateTime(2026, 8, 15, 22, 1, 0);

        var fechaFin =
            fechaInicio.AddHours(3);


        var exception = Assert.Throws<DomainException>(() =>
            new Event(
                Guid.NewGuid(),
                EventType.Concierto,
                "Evento sábado noche",
                fechaInicio,
                fechaFin,
                50000,
                1000));


        Assert.Equal(
            "Los eventos de fin de semana no pueden iniciar después de las 22:00.",
            exception.Message);
    }


    [Fact]
    public void CrearEvento_SabadoExactamente22_DebeSerValido()
    {
        var fechaInicio =
            new DateTime(2026, 8, 15, 22, 0, 0);

        var fechaFin =
            fechaInicio.AddHours(3);


        var evento = new Event(
            Guid.NewGuid(),
            EventType.Concierto,
            "Evento sábado permitido",
            fechaInicio,
            fechaFin,
            50000,
            1000);


        Assert.NotNull(evento);
    }


    [Fact]
    public void CrearEvento_DiaSemanaDespuesDeLas22_DebeSerValido()
    {
        var fechaInicio =
            new DateTime(2026, 8, 13, 23, 0, 0);

        var fechaFin =
            fechaInicio.AddHours(2);


        var evento = new Event(
            Guid.NewGuid(),
            EventType.Concierto,
            "Evento jueves noche",
            fechaInicio,
            fechaFin,
            50000,
            1000);


        Assert.NotNull(evento);
    }
}