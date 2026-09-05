using CuentasService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CuentasService.API.Controllers;

/// <summary>
/// F4: GET /reportes?fecha=2022-01-01,2022-12-31&amp;cliente=CLI001
/// El parámetro `fecha` recibe un rango "desde,hasta"; devuelve el estado de cuenta en JSON.
/// </summary>
[ApiController]
[Route("reportes")]
[Produces("application/json")]
public class ReportesController : ControllerBase
{
    private readonly IReporteAppService _reporteAppService;

    public ReportesController(IReporteAppService reporteAppService)
    {
        _reporteAppService = reporteAppService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerEstadoCuenta(
        [FromQuery] string fecha, [FromQuery] string cliente, CancellationToken cancellationToken)
    {
        var partes = fecha.Split(',', StringSplitOptions.TrimEntries);
        if (partes.Length != 2 || !DateTime.TryParse(partes[0], out var desde) || !DateTime.TryParse(partes[1], out var hasta))
        {
            return BadRequest(new
            {
                title = "Parámetro 'fecha' inválido",
                detail = "Use el formato fecha=YYYY-MM-DD,YYYY-MM-DD (rango desde,hasta)."
            });
        }

        var desdeUtc = DateTime.SpecifyKind(desde, DateTimeKind.Utc);
        var hastaUtc = DateTime.SpecifyKind(hasta.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

        var reporte = await _reporteAppService.GenerarEstadoCuentaAsync(cliente, desdeUtc, hastaUtc, cancellationToken);
        return Ok(reporte);
    }
}
