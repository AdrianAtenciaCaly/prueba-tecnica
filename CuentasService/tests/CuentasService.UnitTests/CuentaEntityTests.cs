using CuentasService.Domain.Entities;
using CuentasService.Domain.Enums;
using CuentasService.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace CuentasService.UnitTests;

/// <summary>
/// F5: Pruebas unitarias de la entidad de dominio Cuenta, en particular F2 (registro de movimientos
/// y actualización de saldo) y F3 (rechazo con "Saldo no disponible" cuando no hay fondos).
/// </summary>
public class CuentaEntityTests
{
    private static Cuenta CrearCuentaConSaldo(decimal saldoInicial = 2000) =>
        Cuenta.Abrir("478758", TipoCuenta.Ahorros, saldoInicial, "CLI001");

    [Fact]
    public void Abrir_ConDatosValidos_DeberiaCrearCuentaConSaldoInicial()
    {
        var cuenta = CrearCuentaConSaldo(2000);

        cuenta.NumeroCuenta.Should().Be("478758");
        cuenta.SaldoActual.Should().Be(2000);
        cuenta.Estado.Should().BeTrue();
    }

    [Fact]
    public void Abrir_ConSaldoInicialNegativo_DeberiaLanzarExcepcion()
    {
        var accion = () => Cuenta.Abrir("478758", TipoCuenta.Ahorros, -100, "CLI001");

        accion.Should().Throw<CuentaDomainException>();
    }

    [Fact]
    public void RegistrarMovimiento_Deposito_DeberiaAumentarSaldo()
    {
        var cuenta = CrearCuentaConSaldo(100);

        var movimiento = cuenta.RegistrarMovimiento(TipoMovimiento.Deposito, 600, DateTime.UtcNow);

        cuenta.SaldoActual.Should().Be(700);
        movimiento.Saldo.Should().Be(700);
        movimiento.TipoMovimiento.Should().Be(TipoMovimiento.Deposito);
    }

    [Fact]
    public void RegistrarMovimiento_RetiroConFondosSuficientes_DeberiaDisminuirSaldo()
    {
        var cuenta = CrearCuentaConSaldo(2000);

        var movimiento = cuenta.RegistrarMovimiento(TipoMovimiento.Retiro, 575, DateTime.UtcNow);

        cuenta.SaldoActual.Should().Be(1425);
        movimiento.Valor.Should().Be(575);
    }

    [Fact]
    public void RegistrarMovimiento_RetiroSinFondosSuficientes_DeberiaLanzarSaldoNoDisponible()
    {
        // Caso de uso del enunciado: cuenta 496825 con saldo 540, retiro de 540... probamos el caso límite excedido.
        var cuenta = CrearCuentaConSaldo(540);

        var accion = () => cuenta.RegistrarMovimiento(TipoMovimiento.Retiro, 541, DateTime.UtcNow);

        accion.Should().Throw<SaldoNoDisponibleException>().WithMessage("Saldo no disponible");
        cuenta.SaldoActual.Should().Be(540); // el saldo no debe modificarse si el movimiento se rechaza
    }

    [Fact]
    public void RegistrarMovimiento_RetiroExactoAlSaldoDisponible_DeberiaDejarSaldoEnCero()
    {
        var cuenta = CrearCuentaConSaldo(540);

        cuenta.RegistrarMovimiento(TipoMovimiento.Retiro, 540, DateTime.UtcNow);

        cuenta.SaldoActual.Should().Be(0);
    }

    [Fact]
    public void RegistrarMovimiento_EnCuentaInactiva_DeberiaLanzarExcepcion()
    {
        var cuenta = CrearCuentaConSaldo(1000);
        cuenta.ActualizarDatos(TipoCuenta.Ahorros, estado: false);

        var accion = () => cuenta.RegistrarMovimiento(TipoMovimiento.Deposito, 100, DateTime.UtcNow);

        accion.Should().Throw<CuentaDomainException>().WithMessage("*inactiva*");
    }

    [Fact]
    public void RegistrarMovimiento_ConValorCeroONegativo_DeberiaLanzarExcepcion()
    {
        var cuenta = CrearCuentaConSaldo(1000);

        var accion = () => cuenta.RegistrarMovimiento(TipoMovimiento.Deposito, 0, DateTime.UtcNow);

        accion.Should().Throw<CuentaDomainException>();
    }
}
