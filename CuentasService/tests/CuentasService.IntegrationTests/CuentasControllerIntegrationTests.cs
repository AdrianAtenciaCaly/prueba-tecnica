using System.Net;
using System.Net.Http.Json;
using CuentasService.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace CuentasService.IntegrationTests;

/// <summary>F6: pruebas de integración de extremo a extremo sobre /cuentas y /movimientos.</summary>
public class CuentasControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CuentasControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCuentas_ConClienteValido_DeberiaCrearCuenta()
    {
        await _factory.SembrarClienteReferenciaAsync("CLI001", "Jose Lema");
        var dto = new CrearCuentaDto("478758", "Ahorros", 2000, "CLI001");

        var response = await _client.PostAsJsonAsync("/cuentas", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var creada = await response.Content.ReadFromJsonAsync<CuentaDto>();
        creada!.SaldoActual.Should().Be(2000);
    }

    [Fact]
    public async Task PostCuentas_ConClienteInexistente_DeberiaRetornar422()
    {
        var dto = new CrearCuentaDto("999999", "Ahorros", 500, "CLI_NO_EXISTE");

        var response = await _client.PostAsJsonAsync("/cuentas", dto);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task PostMovimientos_RetiroSinSaldo_DeberiaRetornar400ConMensajeSaldoNoDisponible()
    {
        await _factory.SembrarClienteReferenciaAsync("CLI002", "Marianela Montalvo");
        var cuentaResponse = await _client.PostAsJsonAsync("/cuentas", new CrearCuentaDto("495878", "Ahorros", 0, "CLI002"));
        var cuenta = await cuentaResponse.Content.ReadFromJsonAsync<CuentaDto>();

        var movimientoResponse = await _client.PostAsJsonAsync("/movimientos",
            new CrearMovimientoDto(cuenta!.Id, "Retiro", 100));

        movimientoResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var contenido = await movimientoResponse.Content.ReadAsStringAsync();
        contenido.Should().Contain("Saldo no disponible");
    }

    [Fact]
    public async Task PostMovimientos_DepositoValido_DeberiaActualizarSaldo()
    {
        await _factory.SembrarClienteReferenciaAsync("CLI003", "Juan Osorio");
        var cuentaResponse = await _client.PostAsJsonAsync("/cuentas", new CrearCuentaDto("496825", "Ahorros", 540, "CLI003"));
        var cuenta = await cuentaResponse.Content.ReadFromJsonAsync<CuentaDto>();

        var movimientoResponse = await _client.PostAsJsonAsync("/movimientos",
            new CrearMovimientoDto(cuenta!.Id, "Deposito", 100));

        movimientoResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var movimiento = await movimientoResponse.Content.ReadFromJsonAsync<MovimientoDto>();
        movimiento!.Saldo.Should().Be(640);
    }
}
