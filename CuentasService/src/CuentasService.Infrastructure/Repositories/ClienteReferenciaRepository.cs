using CuentasService.Domain.Entities;
using CuentasService.Domain.Interfaces;
using CuentasService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CuentasService.Infrastructure.Repositories;

public class ClienteReferenciaRepository : IClienteReferenciaRepository
{
    private readonly CuentasDbContext _context;

    public ClienteReferenciaRepository(CuentasDbContext context)
    {
        _context = context;
    }

    public async Task<ClienteReferencia?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default) =>
        await _context.ClientesReferencia.FirstOrDefaultAsync(c => c.ClienteId == clienteId, cancellationToken);

    public async Task UpsertAsync(string clienteId, string nombre, bool estado, CancellationToken cancellationToken = default)
    {
        var existente = await _context.ClientesReferencia.FirstOrDefaultAsync(c => c.ClienteId == clienteId, cancellationToken);

        if (existente is null)
            await _context.ClientesReferencia.AddAsync(new ClienteReferencia(clienteId, nombre, estado), cancellationToken);
        else
            existente.Actualizar(nombre, estado);
    }

    public async Task MarcarEliminadoAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        var existente = await _context.ClientesReferencia.FirstOrDefaultAsync(c => c.ClienteId == clienteId, cancellationToken);
        existente?.MarcarEliminado();
    }

    public async Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
