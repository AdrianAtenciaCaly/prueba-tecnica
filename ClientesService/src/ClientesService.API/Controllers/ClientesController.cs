using ClientesService.Application.DTOs;
using ClientesService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClientesService.API.Controllers;

/// <summary>
/// F1: CRUD completo de Cliente. Endpoint: /clientes
/// El controlador es deliberadamente "delgado": solo traduce HTTP <-> Application (SRP).
/// Toda la lógica de negocio vive en ClienteAppService / Domain.
/// </summary>
[ApiController]
[Route("clientes")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IClienteAppService _clienteAppService;

    public ClientesController(IClienteAppService clienteAppService)
    {
        _clienteAppService = clienteAppService;
    }

    /// <summary>Crea un nuevo cliente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Crear([FromBody] CrearClienteDto dto, CancellationToken cancellationToken)
    {
        var cliente = await _clienteAppService.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
    }

    /// <summary>Obtiene todos los clientes.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var clientes = await _clienteAppService.ObtenerTodosAsync(cancellationToken);
        return Ok(clientes);
    }

    /// <summary>Obtiene un cliente por su Id (clave primaria).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var cliente = await _clienteAppService.ObtenerPorIdAsync(id, cancellationToken);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    /// <summary>Obtiene un cliente por su ClienteId (clave única de negocio).</summary>
    [HttpGet("por-cliente-id/{clienteId}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorClienteId(string clienteId, CancellationToken cancellationToken)
    {
        var cliente = await _clienteAppService.ObtenerPorClienteIdAsync(clienteId, cancellationToken);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    /// <summary>Actualiza los datos de un cliente existente.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarClienteDto dto, CancellationToken cancellationToken)
    {
        var cliente = await _clienteAppService.ActualizarAsync(id, dto, cancellationToken);
        return Ok(cliente);
    }

    /// <summary>Elimina un cliente.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        await _clienteAppService.EliminarAsync(id, cancellationToken);
        return NoContent();
    }
}
