using CuentasService.Application.DTOs;
using CuentasService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CuentasService.API.Controllers;

/// <summary>registro con validación de saldo. Endpoint: /movimientos</summary>
[ApiController]
[Route("movimientos")]
[Produces("application/json")]
public class MovimientosController : ControllerBase
{
    private readonly IMovimientoAppService _movimientoAppService;

    public MovimientosController(IMovimientoAppService movimientoAppService)
    {
        _movimientoAppService = movimientoAppService;
    }

    /// <summary>
    /// Registra un movimiento (depósito o retiro). Si no hay saldo suficiente, responde 400
    /// con el mensaje "Saldo no disponible".
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MovimientoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] CrearMovimientoDto dto, CancellationToken cancellationToken)
    {
        var movimiento = await _movimientoAppService.RegistrarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(Registrar), new { id = movimiento.Id }, movimiento);
    }
}
