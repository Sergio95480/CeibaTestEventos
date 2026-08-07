using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CeibaTestEventos.IntegrationTests;

public class EventApiTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;


    public EventApiTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task CrearEvento_DatosValidos_RetornaCreated()
    {
        // Crear Venue primero porque el evento depende de un Venue existente
        var venueRequest = new
        {
            nombre = "Venue integración",
            capacidad = 500,
            ciudad = "Bogotá",
            direccion = "Calle 123"
        };


        var venueResponse = await _client.PostAsJsonAsync(
            "/api/Venues",
            venueRequest);


        if (venueResponse.StatusCode != HttpStatusCode.Created)
        {
            var error = await venueResponse.Content.ReadAsStringAsync();

            throw new Exception(
                $"Error creando Venue: {error}");
        }


        var venue = await venueResponse.Content
            .ReadFromJsonAsync<VenueResponse>();


        Assert.NotNull(venue);


        // Crear evento asociado al Venue creado
        var eventRequest = new
        {
            venueId = venue.Id,
            tipoEvento = 3,
            nombre = "Evento integración",
            fechaInicio = DateTime.UtcNow.AddDays(10),
            fechaFin = DateTime.UtcNow.AddDays(10).AddHours(3),
            precio = 50000,
            capacidad = 100
        };


        var eventResponse = await _client.PostAsJsonAsync(
            "/api/Events",
            eventRequest);


        if (eventResponse.StatusCode != HttpStatusCode.Created)
        {
            var error = await eventResponse.Content.ReadAsStringAsync();

            throw new Exception(
                $"Error creando Evento: {error}");
        }


        Assert.Equal(
            HttpStatusCode.Created,
            eventResponse.StatusCode);
    }


    private sealed record VenueResponse(
        Guid Id);
}