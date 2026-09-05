namespace CuentasService.Application.DTOs;

/// <summary>
/// DTO de entrada para F2. Se admite `Valor` con signo (positivo = depósito, negativo = retiro),
/// tal como lo muestran los ejemplos del enunciado ("Retiro de 575" con valores negativos en los reportes),
/// o alternativamente el campo TipoMovimiento explícito. Aquí se usa TipoMovimiento + Valor absoluto
/// para que la intención sea inequívoca en el contrato de API.
/// </summary>
public record CrearMovimientoDto(
    Guid CuentaId,
    string TipoMovimiento,
    decimal Valor,
    DateTime? Fecha = null);
