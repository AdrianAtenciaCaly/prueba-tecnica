using CuentasService.Application.DTOs;
using CuentasService.Application.Interfaces;
using CuentasService.Domain.Entities;
using CuentasService.Domain.Enums;
using CuentasService.Domain.Exceptions;
using CuentasService.Domain.Interfaces;

namespace CuentasService.Application.Services;

/// <summary>F1: casos de uso CRU de Cuenta. Valida contra el read-model de Cliente antes de abrir la cuenta.</summary>
public class CuentaAppService : ICuentaAppService
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IClienteValidator _clienteValidator;

    public CuentaAppService(ICuentaRepository cuentaRepository, IClienteValidator clienteValidator)
    {
        _cuentaRepository = cuentaRepository;
        _clienteValidator = clienteValidator;
    }

    public async Task<CuentaDto> CrearAsync(CrearCuentaDto dto, CancellationToken cancellationToken = default)
    {
        if (await _cuentaRepository.ExisteNumeroCuentaAsync(dto.NumeroCuenta, cancellationToken))
            throw new CuentaDuplicadaException(dto.NumeroCuenta);

        await _clienteValidator.ValidarClienteActivoAsync(dto.ClienteId, cancellationToken);

        var tipoCuenta = ParsearTipoCuenta(dto.TipoCuenta);
        var cuenta = Cuenta.Abrir(dto.NumeroCuenta, tipoCuenta, dto.SaldoInicial, dto.ClienteId, dto.Estado);

        await _cuentaRepository.AgregarAsync(cuenta, cancellationToken);
        await _cuentaRepository.GuardarCambiosAsync(cancellationToken);

        return MapToDto(cuenta);
    }

    public async Task<CuentaDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cuenta = await _cuentaRepository.ObtenerPorIdAsync(id, cancellationToken);
        return cuenta is null ? null : MapToDto(cuenta);
    }

    public async Task<IReadOnlyList<CuentaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var cuentas = await _cuentaRepository.ObtenerTodasAsync(cancellationToken);
        return cuentas.Select(MapToDto).ToList();
    }

    public async Task<CuentaDto> ActualizarAsync(Guid id, ActualizarCuentaDto dto, CancellationToken cancellationToken = default)
    {
        var cuenta = await _cuentaRepository.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new CuentaNoEncontradaException(id);

        cuenta.ActualizarDatos(ParsearTipoCuenta(dto.TipoCuenta), dto.Estado);

        _cuentaRepository.Actualizar(cuenta);
        await _cuentaRepository.GuardarCambiosAsync(cancellationToken);

        return MapToDto(cuenta);
    }

    private static TipoCuenta ParsearTipoCuenta(string valor) =>
        Enum.TryParse<TipoCuenta>(valor, ignoreCase: true, out var tipo)
            ? tipo
            : throw new CuentaDomainException($"Tipo de cuenta inválido: '{valor}'. Use 'Ahorros' o 'Corriente'.");

    private static CuentaDto MapToDto(Cuenta c) =>
        new(c.Id, c.NumeroCuenta, c.TipoCuenta.ToString(), c.SaldoInicial, c.SaldoActual, c.Estado, c.ClienteId);
}
