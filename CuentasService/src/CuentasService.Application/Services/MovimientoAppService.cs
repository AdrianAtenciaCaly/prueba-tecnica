using CuentasService.Application.DTOs;
using CuentasService.Application.Interfaces;
using CuentasService.Domain.Enums;
using CuentasService.Domain.Exceptions;
using CuentasService.Domain.Interfaces;

namespace CuentasService.Application.Services;

/// <summary>
/// F2/F3: registra un movimiento sobre una cuenta. Toda la lógica de saldo/validación vive en la entidad
/// Cuenta (Domain) — este servicio solo orquesta: carga el agregado, invoca el comportamiento de dominio,
/// persiste. Así se evita duplicar reglas de negocio entre capas.
/// </summary>
public class MovimientoAppService : IMovimientoAppService
{
    private readonly ICuentaRepository _cuentaRepository;

    public MovimientoAppService(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<MovimientoDto> RegistrarAsync(CrearMovimientoDto dto, CancellationToken cancellationToken = default)
    {
        var cuenta = await _cuentaRepository.ObtenerPorIdConMovimientosAsync(dto.CuentaId, cancellationToken)
            ?? throw new CuentaNoEncontradaException(dto.CuentaId);

        var tipo = Enum.TryParse<TipoMovimiento>(dto.TipoMovimiento, ignoreCase: true, out var t)
            ? t
            : throw new CuentaDomainException($"Tipo de movimiento inválido: '{dto.TipoMovimiento}'. Use 'Deposito' o 'Retiro'.");

        // F3: si no hay saldo suficiente, Cuenta.RegistrarMovimiento lanza SaldoNoDisponibleException("Saldo no disponible")
        var movimiento = cuenta.RegistrarMovimiento(tipo, Math.Abs(dto.Valor), dto.Fecha ?? DateTime.UtcNow);

        await _cuentaRepository.AgregarMovimientoAsync(movimiento, cancellationToken);
        await _cuentaRepository.GuardarCambiosAsync(cancellationToken);

        return new MovimientoDto(
            movimiento.Id, movimiento.Fecha, movimiento.TipoMovimiento.ToString(),
            movimiento.Valor, movimiento.Saldo, movimiento.CuentaId);
    }
}
