using ClientesService.Domain.Entities;
using ClientesService.Domain.Interfaces;
using ClientesService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientesService.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesDbContext _context;

    public ClienteRepository(ClientesDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Cliente?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default) =>
        await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == clienteId, cancellationToken);

    public async Task<Cliente?> ObtenerPorIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default) =>
        await _context.Clientes.FirstOrDefaultAsync(c => c.Identificacion == identificacion, cancellationToken);

    public async Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default) =>
        await _context.Clientes.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AgregarAsync(Cliente cliente, CancellationToken cancellationToken = default) =>
        await _context.Clientes.AddAsync(cliente, cancellationToken);

    public void Actualizar(Cliente cliente) => _context.Clientes.Update(cliente);

    public void Eliminar(Cliente cliente) => _context.Clientes.Remove(cliente);

    public async Task<bool> ExisteClienteIdAsync(string clienteId, CancellationToken cancellationToken = default) =>
        await _context.Clientes.AnyAsync(c => c.ClienteId == clienteId, cancellationToken);

    public async Task<bool> ExisteIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default) =>
        await _context.Clientes.AnyAsync(c => c.Identificacion == identificacion, cancellationToken);

    public async Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
