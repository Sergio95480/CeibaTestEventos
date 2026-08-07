using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CeibaTestEventos.IntegrationTests;

public class ReservationCancellationApiTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;


    public ReservationCancellationApiTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task CancelarReserva_ConMenosDe48Horas_CambiaEstadoALost()
    {
        // Crear Venue
        var venueRequest = new
        {
            nombre = "Venue cancelación integración",
            ciudad = "Bogotá",
            direccion = "Calle 50",
            capacidad = 500
        };


        var venueResponse = await _client.PostAsJsonAsync(
            "/api/Venues",
            venueRequest);


        Assert.Equal(
            HttpStatusCode.Created,
            venueResponse.StatusCode);


        var venue = await venueResponse.Content
            .ReadFromJsonAsync<VenueResponse>();


        Assert.NotNull(venue);


        // Crear evento con inicio cercano (<48 horas)
        var eventRequest = new
        {
            venueId = venue.Id,
            tipoEvento = 3,
            nombre = "Evento cancelación integración",
            fechaInicio = DateTime.UtcNow.AddHours(24),
            fechaFin = DateTime.UtcNow.AddHours(27),
            precio = 50000,
            capacidad = 100
        };


        var eventResponse = await _client.PostAsJsonAsync(
            "/api/Events",
            eventRequest);


        Assert.Equal(
            HttpStatusCode.Created,
            eventResponse.StatusCode);


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
            compradorEmail = "cliente.cancelacion@test.com",
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


        // Cancelar reserva
        var cancelResponse = await _client.PostAsync(
            $"/api/Reservations/{reserva.Id}/cancel",
            null);


        Assert.Equal(
            HttpStatusCode.OK,
            cancelResponse.StatusCode);


        var reservaCancelada = await cancelResponse.Content
            .ReadFromJsonAsync<ReservationResponse>();


        Assert.NotNull(reservaCancelada);


        // ReservationStatus.Lost = 4
        Assert.Equal(
            4,
            reservaCancelada.Estado);
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