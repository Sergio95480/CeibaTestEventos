using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CeibaTestEventos.IntegrationTests;

public class ReservationApiTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;


    public ReservationApiTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task CrearYConfirmarReserva_FlujoValido_RetornaConfirmed()
    {
        // Crear Venue
        var venueRequest = new
        {
            nombre = "Venue reserva integración",
            ciudad = "Bogotá",
            direccion = "Calle 100",
            capacidad = 500
        };


        var venueResponse = await _client.PostAsJsonAsync(
            "/api/Venues",
            venueRequest);


        var venue = await venueResponse.Content
            .ReadFromJsonAsync<VenueResponse>();


        Assert.NotNull(venue);


        // Crear Evento
        var eventRequest = new
        {
            venueId = venue.Id,
            tipoEvento = 3,
            nombre = "Evento reserva integración",
            fechaInicio = DateTime.UtcNow.AddDays(10),
            fechaFin = DateTime.UtcNow.AddDays(10).AddHours(3),
            precio = 50000,
            capacidad = 100
        };


        var eventResponse = await _client.PostAsJsonAsync(
            "/api/Events",
            eventRequest);


        var evento = await eventResponse.Content
            .ReadFromJsonAsync<EventResponse>();


        Assert.NotNull(evento);


        // Publicar evento
        var publishResponse = await _client.PostAsync(
            $"/api/Events/{evento.Id}/publish",
            null);


        Assert.Equal(
            HttpStatusCode.OK,
            publishResponse.StatusCode);


        // Crear reserva
        var reservationRequest = new
        {
            eventId = evento.Id,
            compradorEmail = "cliente@test.com",
            cantidad = 2
        };


        var reservationResponse = await _client.PostAsJsonAsync(
            "/api/Reservations",
            reservationRequest);


        Assert.Equal(
            HttpStatusCode.Created,
            reservationResponse.StatusCode);


        var reserva = await reservationResponse.Content
            .ReadFromJsonAsync<ReservationResponse>();


        Assert.NotNull(reserva);


        // Confirmar reserva
        var confirmResponse = await _client.PostAsync(
            $"/api/Reservations/{reserva.Id}/confirm",
            null);


        Assert.Equal(
            HttpStatusCode.OK,
            confirmResponse.StatusCode);


        var reservaConfirmada = await confirmResponse.Content
            .ReadFromJsonAsync<ReservationResponse>();


        Assert.Equal(
            2,
            reservaConfirmada!.Estado);
    }


    private sealed record VenueResponse(
        Guid Id);


    private sealed record EventResponse(
        Guid Id);


    private sealed record ReservationResponse(
        Guid Id,
        Guid EventId,
        string CompradorEmail,
        int Cantidad,
        int Estado,
        string? CodigoConfirmacion);
}