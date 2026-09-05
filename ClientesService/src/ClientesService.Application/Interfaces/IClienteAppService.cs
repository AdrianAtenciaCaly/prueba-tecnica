using ClientesService.Application.DTOs;

namespace ClientesService.Application.Interfaces;

public interface IClienteAppService
{
    Task<ClienteDto> CrearAsync(CrearClienteDto dto, CancellationToken cancellationToken = default);
    Task<ClienteDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClienteDto?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<ClienteDto> ActualizarAsync(Guid id, ActualizarClienteDto dto, CancellationToken cancellationToken = default);
    Task EliminarAsync(Guid id, CancellationToken cancellationToken = default);
}
