namespace CuentasService.Domain.Exceptions;

/// <summary>
/// Se lanza cuando se intenta abrir una cuenta para un ClienteId que este microservicio no conoce
/// (aún no ha llegado el evento de integración ClienteCreado) o que está inactivo.
/// </summary>
public class ClienteInvalidoException : CuentaDomainException
{
    public ClienteInvalidoException(string clienteId)
        : base($"El cliente '{clienteId}' no existe o se encuentra inactivo en el sistema.") { }
}
