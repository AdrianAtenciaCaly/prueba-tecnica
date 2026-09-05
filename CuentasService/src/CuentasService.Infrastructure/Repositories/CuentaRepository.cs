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

    /// <summary>
    /// Registra explícitamente un nuevo Movimiento en EF Core como entidad Added (→ INSERT).
    /// El Movimiento pertenece al agregado Cuenta, por eso se persiste a través del mismo
    /// repositorio. El tracking explícito es necesario porque la colección _movimientos usa
    /// PropertyAccessMode.Field (campo privado), y DetectChanges no garantiza detectar
    /// adiciones a colecciones privadas sin un proxy de seguimiento activo.
    /// </summary>
    public async Task AgregarMovimientoAsync(Movimiento movimiento, CancellationToken cancellationToken = default) =>
        await _context.Movimientos.AddAsync(movimiento, cancellationToken);

    /// <summary>
    /// Para entidades DESCONECTADAS (detached): escenarios PUT/PATCH donde la entidad
    /// no fue cargada previamente en el mismo DbContext. Marca toda la entidad como Modified.
    /// No llamar cuando la entidad fue cargada con tracking en el mismo scope.
    /// </summary>
    public void Actualizar(Cuenta cuenta) => _context.Entry(cuenta).State = EntityState.Modified;

    public async Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta, CancellationToken cancellationToken = default) =>
        await _context.Cuentas.AnyAsync(c => c.NumeroCuenta == numeroCuenta, cancellationToken);

    public async Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
