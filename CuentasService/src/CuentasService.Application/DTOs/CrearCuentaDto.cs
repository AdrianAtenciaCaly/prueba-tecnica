namespace CuentasService.Application.DTOs;

public record CrearCuentaDto(
    string NumeroCuenta,
    string TipoCuenta,
    decimal SaldoInicial,
    string ClienteId,
    bool Estado = true);
