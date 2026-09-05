namespace CuentasService.Domain.Exceptions;

public class ClienteInvalidoException : CuentaDomainException
{
    public ClienteInvalidoException(string clienteId)
        : base($"El cliente '{clienteId}' no existe o se encuentra inactivo en el sistema.") { }
}
