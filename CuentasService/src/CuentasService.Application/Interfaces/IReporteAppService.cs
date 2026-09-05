using CuentasService.Application.DTOs;

namespace CuentasService.Application.Interfaces;

/// <summary>F4: reporte de estado de cuenta por cliente y rango de fechas.</summary>
public interface IReporteAppService
{
    Task<ReporteEstadoCuentaDto> GenerarEstadoCuentaAsync(
        string clienteId, DateTime desde, DateTime hasta, CancellationToken cancellationToken = default);
}
