using System.Net;
using System.Net.Http.Json;
using ClientesService.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace ClientesService.IntegrationTests;

/// <summary>
/// F6: Prueba de integración de extremo a extremo sobre el endpoint /clientes: HTTP -> Middleware ->
/// Controller -> Application -> Repository -> DbContext (InMemory), validando el flujo completo real.
/// </summary>
public class ClientesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ClientesControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostClientes_ConDatosValidos_DeberiaCrearClienteYRetornar201()
    {
        var dto = new CrearClienteDto("CLI100", "Juan Osorio", "Masculino", 40, "1099887766", "13 junio y Equinoccial", "0988745871", "clave123");

        var response = await _client.PostAsJsonAsync("/clientes", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var creado = await response.Content.ReadFromJsonAsync<ClienteDto>();
        creado!.ClienteId.Should().Be("CLI100");
    }

    [Fact]
    public async Task PostClientes_ConClienteIdDuplicado_DeberiaRetornar409()
    {
        var dto = new CrearClienteDto("CLI200", "Marianela Montalvo", "Femenino", 32, "1055443322", "Amazonas y NNUU", "0975489655", "clave123");

        await _client.PostAsJsonAsync("/clientes", dto);
        var segundaRespuesta = await _client.PostAsJsonAsync("/clientes", dto);

        segundaRespuesta.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetClientePorId_Inexistente_DeberiaRetornar404()
    {
        var response = await _client.GetAsync($"/clientes/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetClientes_DeberiaRetornar200ConListaDeClientes()
    {
        await _client.PostAsJsonAsync("/clientes",
            new CrearClienteDto("CLI300", "Ana Torres", "Femenino", 28, "1088776655", "Av. Central", "0991122334", "clave123"));

        var response = await _client.GetAsync("/clientes");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var clientes = await response.Content.ReadFromJsonAsync<List<ClienteDto>>();
        clientes.Should().NotBeNull();
    }
}
