using CuentasService.Domain.Entities;

namespace CuentasService.Domain.Interfaces;

public interface IMovimientoRepository
{
    Task<Movimiento?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Usado por F4 (Reportes): movimientos de las cuentas de un cliente en un rango de fechas.</summary>
    Task<IReadOnlyList<Movimiento>> ObtenerPorCuentasYRangoAsync(
        IEnumerable<Guid> cuentaIds, DateTime desde, DateTime hasta, CancellationToken cancellationToken = default);
}
