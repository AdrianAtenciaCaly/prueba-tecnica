namespace ClientesService.Domain.Exceptions;

/// <summary>Se lanza cuando se busca un cliente que no existe.</summary>
public class ClienteNoEncontradoException : ClienteDomainException
{
    public ClienteNoEncontradoException(string clienteId)
        : base($"No se encontró un cliente con ClienteId '{clienteId}'.") { }

    public ClienteNoEncontradoException(Guid id)
        : base($"No se encontró un cliente con Id '{id}'.") { }
}
