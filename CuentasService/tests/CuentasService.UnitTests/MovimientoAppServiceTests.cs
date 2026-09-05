using CuentasService.Application.DTOs;
using CuentasService.Application.Services;
using CuentasService.Domain.Entities;
using CuentasService.Domain.Enums;
using CuentasService.Domain.Exceptions;
using CuentasService.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CuentasService.UnitTests;

/// <summary>Pruebas unitarias del servicio de aplicación de Movimientos, con el repositorio mockeado.</summary>
public class MovimientoAppServiceTests
{
    private readonly Mock<ICuentaRepository> _cuentaRepositoryMock = new();
    private readonly MovimientoAppService _sut;

    public MovimientoAppServiceTests()
    {
        _sut = new MovimientoAppService(_cuentaRepositoryMock.Object);
    }

    [Fact]
    public async Task RegistrarAsync_ConCuentaInexistente_DeberiaLanzarExcepcion()
    {
        _cuentaRepositoryMock.Setup(r => r.ObtenerPorIdConMovimientosAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Cuenta?)null);

        var dto = new CrearMovimientoDto(Guid.NewGuid(), "Deposito", 100);

        var accion = async () => await _sut.RegistrarAsync(dto);

        await accion.Should().ThrowAsync<CuentaNoEncontradaException>();
    }

    [Fact]
    public async Task RegistrarAsync_RetiroSinSaldo_DeberiaLanzarSaldoNoDisponibleYNoGuardar()
    {
        var cuenta = Cuenta.Abrir("495878", TipoCuenta.Ahorros, 0, "CLI002");
        _cuentaRepositoryMock.Setup(r => r.ObtenerPorIdConMovimientosAsync(cuenta.Id, default)).ReturnsAsync(cuenta);

        var dto = new CrearMovimientoDto(cuenta.Id, "Retiro", 50);

        var accion = async () => await _sut.RegistrarAsync(dto);

        await accion.Should().ThrowAsync<SaldoNoDisponibleException>();
        _cuentaRepositoryMock.Verify(r => r.GuardarCambiosAsync(default), Times.Never);
    }

    [Fact]
    public async Task RegistrarAsync_DepositoValido_DeberiaGuardarCambios()
    {
        var cuenta = Cuenta.Abrir("225487", TipoCuenta.Corriente, 100, "CLI003");
        _cuentaRepositoryMock.Setup(r => r.ObtenerPorIdConMovimientosAsync(cuenta.Id, default)).ReturnsAsync(cuenta);

        var dto = new CrearMovimientoDto(cuenta.Id, "Deposito", 600);

        var resultado = await _sut.RegistrarAsync(dto);

        resultado.Saldo.Should().Be(700);
        _cuentaRepositoryMock.Verify(r => r.GuardarCambiosAsync(default), Times.Once);
    }
}
