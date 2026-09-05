using CuentasService.Application.DTOs;

namespace CuentasService.Application.Interfaces;
public interface IReporteAppService
{
    Task<ReporteEstadoCuentaDto> GenerarEstadoCuentaAsync(
        string clienteId, DateTime desde, DateTime hasta, CancellationToken cancellationToken = default);
}
