using CuentasService.Domain.Entities;
using CuentasService.Domain.Interfaces;
using CuentasService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CuentasService.Infrastructure.Repositories;

public class CuentaRepository : ICuentaRepository
{
    private readonly CuentasDbContext _context;

    public CuentaRepository(CuentasDbContext context)
    {
        _context = context;
    }

    public async Task<Cuenta?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Cuentas.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Cuenta?> ObtenerPorIdConMovimientosAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Cuentas.Include(c => c.Movimientos).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Cuenta?> ObtenerPorNumeroCuentaAsync(string numeroCuenta, CancellationToken cancellationToken = default) =>
        await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta, cancellationToken);

    public async Task<IReadOnlyList<Cuenta>> ObtenerTodasAsync(CancellationToken cancellationToken = default) =>
        await _context.Cuentas.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Cuenta>> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default) =>
        await _context.Cuentas.AsNoTracking().Where(c => c.ClienteId == clienteId).ToListAsync(cancellationToken);

    public async Task AgregarAsync(Cuenta cuenta, CancellationToken cancellationToken = default) =>
        await _context.Cuentas.AddAsync(cuenta, cancellationToken);

    public async Task AgregarMovimientoAsync(Movimiento movimiento, CancellationToken cancellationToken = default) =>
        await _context.Movimientos.AddAsync(movimiento, cancellationToken);

    public void Actualizar(Cuenta cuenta) => _context.Entry(cuenta).State = EntityState.Modified;

    public async Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta, CancellationToken cancellationToken = default) =>
        await _context.Cuentas.AnyAsync(c => c.NumeroCuenta == numeroCuenta, cancellationToken);

    public async Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
