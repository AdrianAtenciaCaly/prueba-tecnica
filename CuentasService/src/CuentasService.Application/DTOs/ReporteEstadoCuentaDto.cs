namespace CuentasService.Application.DTOs;
public record ReporteEstadoCuentaDto(
    string ClienteId,
    string ClienteNombre,
    DateTime FechaDesde,
    DateTime FechaHasta,
    IReadOnlyList<CuentaConMovimientosDto> Cuentas);

public record CuentaConMovimientosDto(
    string NumeroCuenta,
    string TipoCuenta,
    decimal SaldoInicial,
    decimal SaldoActual,
    bool Estado,
    IReadOnlyList<MovimientoReporteDto> Movimientos);

public record MovimientoReporteDto(
    DateTime Fecha,
    string TipoMovimiento,
    decimal Valor,
    decimal Saldo);
