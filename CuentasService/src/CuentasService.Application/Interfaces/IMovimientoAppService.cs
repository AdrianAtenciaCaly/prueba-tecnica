using CuentasService.Application.DTOs;

namespace CuentasService.Application.Interfaces;

/// <summary>F2/F3: registro de movimientos con actualización de saldo y validación de fondos.</summary>
public interface IMovimientoAppService
{
    Task<MovimientoDto> RegistrarAsync(CrearMovimientoDto dto, CancellationToken cancellationToken = default);
}
