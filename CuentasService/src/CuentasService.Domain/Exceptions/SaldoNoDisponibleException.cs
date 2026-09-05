namespace CuentasService.Domain.Exceptions;

/// <summary>
/// F3 (requisito explícito del ejercicio): al intentar un movimiento sin saldo suficiente,
/// se debe alertar con el mensaje "Saldo no disponible".
/// </summary>
public class SaldoNoDisponibleException : CuentaDomainException
{
    public SaldoNoDisponibleException() : base("Saldo no disponible") { }
}
