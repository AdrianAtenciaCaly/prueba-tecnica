using CuentasService.Application.DTOs;
using CuentasService.Application.Interfaces;
using CuentasService.Domain.Exceptions;
using CuentasService.Domain.Interfaces;

namespace CuentasService.Application.Services;

/// <summary>
/// F4: genera el reporte de "Estado de cuenta" para un cliente en un rango de fechas.
/// GET /reportes?fecha={desde},{hasta}&cliente={clienteId} -> JSON con cuentas y sus movimientos.
/// </summary>
public class ReporteAppService : IReporteAppService
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;

    public ReporteAppService(
        ICuentaRepository cuentaRepository,
        IMovimientoRepository movimientoRepository,
        IClienteReferenciaRepository clienteReferenciaRepository)
    {
        _cuentaRepository = cuentaRepository;
        _movimientoRepository = movimientoRepository;
        _clienteReferenciaRepository = clienteReferenciaRepository;
    }

    public async Task<ReporteEstadoCuentaDto> GenerarEstadoCuentaAsync(
        string clienteId, DateTime desde, DateTime hasta, CancellationToken cancellationToken = default)
    {
        var clienteReferencia = await _clienteReferenciaRepository.ObtenerPorClienteIdAsync(clienteId, cancellationToken)
            ?? throw new ClienteInvalidoException(clienteId);

        var cuentas = await _cuentaRepository.ObtenerPorClienteIdAsync(clienteId, cancellationToken);

        var movimientos = await _movimientoRepository.ObtenerPorCuentasYRangoAsync(
            cuentas.Select(c => c.Id), desde, hasta, cancellationToken);

        var cuentasConMovimientos = cuentas.Select(cuenta => new CuentaConMovimientosDto(
            cuenta.NumeroCuenta,
            cuenta.TipoCuenta.ToString(),
            cuenta.SaldoInicial,
            cuenta.SaldoActual,
            cuenta.Estado,
            movimientos
                .Where(m => m.CuentaId == cuenta.Id)
                .OrderBy(m => m.Fecha)
                .Select(m => new MovimientoReporteDto(m.Fecha, m.TipoMovimiento.ToString(), m.Valor, m.Saldo))
                .ToList()
        )).ToList();

        return new ReporteEstadoCuentaDto(clienteId, clienteReferencia.Nombre, desde, hasta, cuentasConMovimientos);
    }
}
