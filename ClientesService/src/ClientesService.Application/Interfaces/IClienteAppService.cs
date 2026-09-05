using ClientesService.Application.DTOs;

namespace ClientesService.Application.Interfaces;

/// <summary>
/// Puerto de entrada de la capa de Aplicación (Interface Segregation + Dependency Inversion):
/// la API depende de esta abstracción, no de la implementación concreta.
/// </summary>
public interface IClienteAppService
{
    Task<ClienteDto> CrearAsync(CrearClienteDto dto, CancellationToken cancellationToken = default);
    Task<ClienteDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClienteDto?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<ClienteDto> ActualizarAsync(Guid id, ActualizarClienteDto dto, CancellationToken cancellationToken = default);
    Task EliminarAsync(Guid id, CancellationToken cancellationToken = default);
}
