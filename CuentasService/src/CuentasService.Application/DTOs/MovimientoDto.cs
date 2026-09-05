namespace CuentasService.Application.DTOs;

public record MovimientoDto(
    Guid Id,
    DateTime Fecha,
    string TipoMovimiento,
    decimal Valor,
    decimal Saldo,
    Guid CuentaId);
