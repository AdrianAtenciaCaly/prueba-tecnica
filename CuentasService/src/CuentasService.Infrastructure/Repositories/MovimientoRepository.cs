using CuentasService.Domain.Entities;
using CuentasService.Domain.Interfaces;
using CuentasService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CuentasService.Infrastructure.Repositories;

public class MovimientoRepository : IMovimientoRepository
{
    private readonly CuentasDbContext _context;

    public MovimientoRepository(CuentasDbContext context)
    {
        _context = context;
    }

    public async Task<Movimiento?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Movimientos.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Movimiento>> ObtenerPorCuentasYRangoAsync(
        IEnumerable<Guid> cuentaIds, DateTime desde, DateTime hasta, CancellationToken cancellationToken = default) =>
        await _context.Movimientos
            .AsNoTracking()
            .Where(m => cuentaIds.Contains(m.CuentaId) && m.Fecha >= desde && m.Fecha <= hasta)
            .ToListAsync(cancellationToken);
}
