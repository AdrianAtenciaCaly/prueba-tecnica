namespace CuentasService.Domain.Exceptions;

public class CuentaNoEncontradaException : CuentaDomainException
{
    public CuentaNoEncontradaException(Guid id) : base($"No se encontró una cuenta con Id '{id}'.") { }
    public CuentaNoEncontradaException(string numeroCuenta) : base($"No se encontró la cuenta número '{numeroCuenta}'.") { }
}
