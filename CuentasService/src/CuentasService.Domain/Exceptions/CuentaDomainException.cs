namespace CuentasService.Domain.Exceptions;

/// <summary>Excepción base para violaciones de reglas de negocio del dominio Cuenta/Movimiento.</summary>
public class CuentaDomainException : Exception
{
    public CuentaDomainException(string message) : base(message) { }
}
