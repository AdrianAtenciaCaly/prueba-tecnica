namespace CuentasService.Application.Interfaces;
public interface IClienteValidator
{
    Task ValidarClienteActivoAsync(string clienteId, CancellationToken cancellationToken = default);
}
