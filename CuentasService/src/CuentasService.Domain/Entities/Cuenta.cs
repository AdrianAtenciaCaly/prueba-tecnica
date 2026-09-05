using CuentasService.Domain.Enums;
using CuentasService.Domain.Exceptions;

namespace CuentasService.Domain.Entities;

public class Cuenta
{
    public Guid Id { get; private set; }
    public string NumeroCuenta { get; private set; } = default!;
    public TipoCuenta TipoCuenta { get; private set; }
    public decimal SaldoInicial { get; private set; }
    public decimal SaldoActual { get; private set; }
    public bool Estado { get; private set; }
    public string ClienteId { get; private set; } = default!;

    private readonly List<Movimiento> _movimientos = new();
    public IReadOnlyCollection<Movimiento> Movimientos => _movimientos.AsReadOnly();

    private Cuenta() { } // EF Core

    private Cuenta(string numeroCuenta, TipoCuenta tipoCuenta, decimal saldoInicial, bool estado, string clienteId)
    {
        Id = Guid.NewGuid();
        SetNumeroCuenta(numeroCuenta);
        TipoCuenta = tipoCuenta;

        if (saldoInicial < 0)
            throw new CuentaDomainException("El saldo inicial no puede ser negativo.");

        SaldoInicial = saldoInicial;
        SaldoActual = saldoInicial;
        Estado = estado;

        if (string.IsNullOrWhiteSpace(clienteId))
            throw new CuentaDomainException("La cuenta debe estar asociada a un ClienteId válido.");
        ClienteId = clienteId;
    }

    public static Cuenta Abrir(string numeroCuenta, TipoCuenta tipoCuenta, decimal saldoInicial, string clienteId, bool estado = true) =>
        new(numeroCuenta, tipoCuenta, saldoInicial, estado, clienteId);

    public void SetNumeroCuenta(string numeroCuenta)
    {
        if (string.IsNullOrWhiteSpace(numeroCuenta))
            throw new CuentaDomainException("El número de cuenta no puede estar vacío.");
        NumeroCuenta = numeroCuenta.Trim();
    }

    public void ActualizarDatos(TipoCuenta tipoCuenta, bool estado)
    {
        TipoCuenta = tipoCuenta;
        Estado = estado;
    }

    /// <summary>
    /// F2/F3: Registra un movimiento (positivo = depósito, negativo = retiro), valida saldo suficiente
    /// para retiros y actualiza el saldo disponible de forma atómica junto con el movimiento (invariante
    /// de la entidad: SaldoActual siempre refleja la suma de movimientos sobre el saldo inicial).
    /// </summary>
    public Movimiento RegistrarMovimiento(TipoMovimiento tipo, decimal valor, DateTime fecha)
    {
        if (!Estado)
            throw new CuentaDomainException("No se pueden registrar movimientos en una cuenta inactiva.");

        if (valor <= 0)
            throw new CuentaDomainException("El valor del movimiento debe ser mayor a cero.");

        var valorConSigno = tipo == TipoMovimiento.Retiro ? -valor : valor;

        if (SaldoActual + valorConSigno < 0)
            throw new SaldoNoDisponibleException();

        SaldoActual += valorConSigno;

        var movimiento = Movimiento.Crear(Id, fecha, tipo, valor, SaldoActual);
        _movimientos.Add(movimiento);
        return movimiento;
    }
}
