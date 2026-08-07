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
}