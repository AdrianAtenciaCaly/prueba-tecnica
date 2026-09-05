namespace CuentasService.Domain.Exceptions;

public class SaldoNoDisponibleException : CuentaDomainException
{
    public SaldoNoDisponibleException() : base("Saldo no disponible") { }
}
