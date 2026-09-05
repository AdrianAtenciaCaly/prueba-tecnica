using CuentasService.Application.DTOs;
using CuentasService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CuentasService.API.Controllers;

/// <summary>F1: CRU (Crear, Leer, Actualizar) sobre Cuenta. Endpoint: /cuentas</summary>
[ApiController]
[Route("cuentas")]
[Produces("application/json")]
public class CuentasController : ControllerBase
{
    private readonly ICuentaAppService _cuentaAppService;

    public CuentasController(ICuentaAppService cuentaAppService)
    {
        _cuentaAppService = cuentaAppService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CuentaDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Crear([FromBody] CrearCuentaDto dto, CancellationToken cancellationToken)
    {
        var cuenta = await _cuentaAppService.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cuenta.Id }, cuenta);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CuentaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodas(CancellationToken cancellationToken)
    {
        var cuentas = await _cuentaAppService.ObtenerTodasAsync(cancellationToken);
        return Ok(cuentas);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CuentaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var cuenta = await _cuentaAppService.ObtenerPorIdAsync(id, cancellationToken);
        return cuenta is null ? NotFound() : Ok(cuenta);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CuentaDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarCuentaDto dto, CancellationToken cancellationToken)
    {
        var cuenta = await _cuentaAppService.ActualizarAsync(id, dto, cancellationToken);
        return Ok(cuenta);
    }
}
