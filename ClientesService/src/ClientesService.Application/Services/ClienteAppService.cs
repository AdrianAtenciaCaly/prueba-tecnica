using ClientesService.Application.DTOs;
using ClientesService.Application.Interfaces;
using ClientesService.Domain.Entities;
using ClientesService.Domain.Exceptions;
using ClientesService.Domain.Interfaces;
using Shared.Contracts;

namespace ClientesService.Application.Services;

public class ClienteAppService : IClienteAppService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IPasswordHasher _passwordHasher;

    public ClienteAppService(
        IClienteRepository clienteRepository,
        IEventPublisher eventPublisher,
        IPasswordHasher passwordHasher)
    {
        _clienteRepository = clienteRepository;
        _eventPublisher = eventPublisher;
        _passwordHasher = passwordHasher;
    }

    public async Task<ClienteDto> CrearAsync(CrearClienteDto dto, CancellationToken cancellationToken = default)
    {
        if (await _clienteRepository.ExisteClienteIdAsync(dto.ClienteId, cancellationToken))
            throw new ClienteIdentificacionDuplicadaException(dto.ClienteId);

        if (await _clienteRepository.ExisteIdentificacionAsync(dto.Identificacion, cancellationToken))
            throw new ClienteIdentificacionDuplicadaException(dto.Identificacion);

        var hash = _passwordHasher.Hash(dto.Contrasena);

        var cliente = Cliente.Crear(
            dto.ClienteId, dto.Nombre, dto.Genero, dto.Edad,
            dto.Identificacion, dto.Direccion, dto.Telefono,
            hash, dto.Estado);

        await _clienteRepository.AgregarAsync(cliente, cancellationToken);
        await _clienteRepository.GuardarCambiosAsync(cancellationToken);

        // Comunicación asíncrona con CuentasService: se publica el evento después de persistir (consistencia eventual).
        await _eventPublisher.PublicarAsync(
            new ClienteCreadoIntegrationEvent(cliente.ClienteId, cliente.Nombre, cliente.Estado, DateTime.UtcNow),
            cancellationToken);

        return MapToDto(cliente);
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id, cancellationToken);
        return cliente is null ? null : MapToDto(cliente);
    }

    public async Task<ClienteDto?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.ObtenerPorClienteIdAsync(clienteId, cancellationToken);
        return cliente is null ? null : MapToDto(cliente);
    }

    public async Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var clientes = await _clienteRepository.ObtenerTodosAsync(cancellationToken);
        return clientes.Select(MapToDto).ToList();
    }

    public async Task<ClienteDto> ActualizarAsync(Guid id, ActualizarClienteDto dto, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new ClienteNoEncontradoException(id);

        cliente.ActualizarDatos(dto.Nombre, dto.Genero, dto.Edad, dto.Direccion, dto.Telefono);

        if (dto.Estado) cliente.Activar(); else cliente.Desactivar();

        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambiosAsync(cancellationToken);

        await _eventPublisher.PublicarAsync(
            new ClienteActualizadoIntegrationEvent(cliente.ClienteId, cliente.Nombre, cliente.Estado, DateTime.UtcNow),
            cancellationToken);

        return MapToDto(cliente);
    }

    public async Task EliminarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new ClienteNoEncontradoException(id);

        _clienteRepository.Eliminar(cliente);
        await _clienteRepository.GuardarCambiosAsync(cancellationToken);

        await _eventPublisher.PublicarAsync(
            new ClienteEliminadoIntegrationEvent(cliente.ClienteId, DateTime.UtcNow),
            cancellationToken);
    }

    private static ClienteDto MapToDto(Cliente c) =>
        new(c.Id, c.ClienteId, c.Nombre, c.Genero, c.Edad, c.Identificacion, c.Direccion, c.Telefono, c.Estado);
}
