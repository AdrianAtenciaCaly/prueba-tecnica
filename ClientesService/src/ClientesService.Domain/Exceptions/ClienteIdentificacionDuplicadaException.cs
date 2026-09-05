namespace ClientesService.Domain.Exceptions;

/// <summary>Se lanza al intentar registrar una identificación o ClienteId que ya existe (violación de clave única).</summary>
public class ClienteIdentificacionDuplicadaException : ClienteDomainException
{
    public ClienteIdentificacionDuplicadaException(string valor)
        : base($"Ya existe un cliente registrado con el valor '{valor}'.") { }
}
