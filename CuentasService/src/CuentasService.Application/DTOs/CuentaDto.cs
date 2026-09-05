namespace CuentasService.Application.DTOs;

public record CuentaDto(
    Guid Id,
    string NumeroCuenta,
    string TipoCuenta,
    decimal SaldoInicial,
    decimal SaldoActual,
    bool Estado,
    string ClienteId);
