namespace CuentasService.Domain.Exceptions;

public class CuentaDomainException : Exception
{
    public CuentaDomainException(string message) : base(message) { }
}
