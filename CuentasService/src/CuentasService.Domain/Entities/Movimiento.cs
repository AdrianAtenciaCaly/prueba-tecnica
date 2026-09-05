using CuentasService.Domain.Enums;
using CuentasService.Domain.Exceptions;

namespace CuentasService.Domain.Entities;

public class Movimiento
{
    public Guid Id { get; private set; }
    public DateTime Fecha { get; private set; }
    public TipoMovimiento TipoMovimiento { get; private set; }
    public decimal Valor { get; private set; }
    public decimal Saldo { get; private set; }
    public Guid CuentaId { get; private set; }

    private Movimiento() { } // EF Core

    private Movimiento(Guid cuentaId, DateTime fecha, TipoMovimiento tipoMovimiento, decimal valor, decimal saldoResultante)
    {
        Id = Guid.NewGuid();
        CuentaId = cuentaId;
        Fecha = fecha;

        if (valor <= 0)
            throw new CuentaDomainException("El valor del movimiento debe ser mayor a cero.");

        TipoMovimiento = tipoMovimiento;
        Valor = valor;
        Saldo = saldoResultante;
    }

    internal static Movimiento Crear(Guid cuentaId, DateTime fecha, TipoMovimiento tipoMovimiento, decimal valor, decimal saldoResultante) =>
        new(cuentaId, fecha, tipoMovimiento, valor, saldoResultante);
}
