namespace CuentasService.Domain.Exceptions;

public class CuentaDuplicadaException : CuentaDomainException
{
    public CuentaDuplicadaException(string numeroCuenta)
        : base($"Ya existe una cuenta registrada con el número '{numeroCuenta}'.") { }
}
