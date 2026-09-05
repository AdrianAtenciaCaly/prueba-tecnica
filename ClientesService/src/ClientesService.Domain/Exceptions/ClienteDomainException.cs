namespace ClientesService.Domain.Exceptions;

/// <summary>Excepción base para violaciones de reglas de negocio del dominio Cliente/Persona.</summary>
public class ClienteDomainException : Exception
{
    public ClienteDomainException(string message) : base(message) { }
}
