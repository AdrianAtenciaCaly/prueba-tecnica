using CuentasService.Domain.Entities;

namespace CuentasService.Domain.Interfaces;

public interface ICuentaRepository
{
    Task<Cuenta?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Cuenta?> ObtenerPorIdConMovimientosAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Cuenta?> ObtenerPorNumeroCuentaAsync(string numeroCuenta, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Cuenta>> ObtenerTodasAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Cuenta>> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);
    Task AgregarAsync(Cuenta cuenta, CancellationToken cancellationToken = default);

    /// <summary>
    /// Registra un nuevo Movimiento en el contexto de EF Core (INSERT).
    /// El Movimiento es parte del agregado Cuenta, por lo que se persiste
    /// a través del mismo repositorio. El tracking explícito evita depender
    /// de DetectChanges sobre colecciones con PropertyAccessMode.Field.
    /// </summary>
    Task AgregarMovimientoAsync(Movimiento movimiento, CancellationToken cancellationToken = default);

    void Actualizar(Cuenta cuenta);
    Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta, CancellationToken cancellationToken = default);
    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default);
}
