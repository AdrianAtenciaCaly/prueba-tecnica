using CuentasService.Application.DTOs;

namespace CuentasService.Application.Interfaces;
public interface ICuentaAppService
{
    Task<CuentaDto> CrearAsync(CrearCuentaDto dto, CancellationToken cancellationToken = default);
    Task<CuentaDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CuentaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);
    Task<CuentaDto> ActualizarAsync(Guid id, ActualizarCuentaDto dto, CancellationToken cancellationToken = default);
}
