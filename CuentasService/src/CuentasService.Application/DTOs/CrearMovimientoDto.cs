namespace CuentasService.Application.DTOs;

public record CrearMovimientoDto(
    Guid CuentaId,
    string TipoMovimiento,
    decimal Valor,
    DateTime? Fecha = null);
