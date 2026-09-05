using CuentasService.Domain.Entities;

namespace CuentasService.Domain.Interfaces;

public interface IMovimientoRepository
{
    Task<Movimiento?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Movimiento>> ObtenerPorCuentasYRangoAsync(
        IEnumerable<Guid> cuentaIds, DateTime desde, DateTime hasta, CancellationToken cancellationToken = default);
}
