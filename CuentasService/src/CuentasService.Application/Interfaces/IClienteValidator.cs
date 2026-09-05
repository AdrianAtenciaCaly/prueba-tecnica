namespace CuentasService.Application.Interfaces;

/// <summary>
/// Abstracción para validar que un ClienteId existe y está activo, respaldada por el read-model local
/// alimentado vía eventos asíncronos (ver ClienteReferenciaRepository). Se aísla en su propia interfaz
/// (Interface Segregation) porque es una preocupación distinta a la persistencia de Cuenta/Movimiento.
/// </summary>
public interface IClienteValidator
{
    Task ValidarClienteActivoAsync(string clienteId, CancellationToken cancellationToken = default);
}
