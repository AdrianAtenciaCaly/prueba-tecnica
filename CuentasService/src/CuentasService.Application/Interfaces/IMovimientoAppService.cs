using CuentasService.Application.DTOs;

namespace CuentasService.Application.Interfaces;

public interface IMovimientoAppService
{
    Task<MovimientoDto> RegistrarAsync(CrearMovimientoDto dto, CancellationToken cancellationToken = default);
}
