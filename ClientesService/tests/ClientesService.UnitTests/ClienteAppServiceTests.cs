using ClientesService.Application.DTOs;
using ClientesService.Application.Interfaces;
using ClientesService.Application.Services;
using ClientesService.Domain.Entities;
using ClientesService.Domain.Exceptions;
using ClientesService.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared.Contracts;
using Xunit;

namespace ClientesService.UnitTests;

/// <summary>
/// Pruebas unitarias del servicio de aplicación usando Moq para aislar el repositorio, el publicador de
/// eventos y el hasher (Dependency Inversion habilita esta testabilidad sin tocar infraestructura real).
/// </summary>
public class ClienteAppServiceTests
{
    private readonly Mock<IClienteRepository> _repoMock = new();
    private readonly Mock<IEventPublisher> _publisherMock = new();
    private readonly Mock<IPasswordHasher> _hasherMock = new();
    private readonly ClienteAppService _sut;

    public ClienteAppServiceTests()
    {
        _hasherMock.Setup(h => h.Hash(It.IsAny<string>())).Returns("hash-simulado");
        _sut = new ClienteAppService(_repoMock.Object, _publisherMock.Object, _hasherMock.Object);
    }

    [Fact]
    public async Task CrearAsync_ConDatosNuevos_DeberiaGuardarYPublicarEvento()
    {
        var dto = new CrearClienteDto("CLI001", "Jose Lema", "Masculino", 35, "1023456789", "Dir", "098", "secreta1");
        _repoMock.Setup(r => r.ExisteClienteIdAsync(dto.ClienteId, default)).ReturnsAsync(false);
        _repoMock.Setup(r => r.ExisteIdentificacionAsync(dto.Identificacion, default)).ReturnsAsync(false);

        var resultado = await _sut.CrearAsync(dto);

        resultado.ClienteId.Should().Be("CLI001");
        _repoMock.Verify(r => r.AgregarAsync(It.IsAny<Cliente>(), default), Times.Once);
        _repoMock.Verify(r => r.GuardarCambiosAsync(default), Times.Once);
        _publisherMock.Verify(p => p.PublicarAsync(It.IsAny<ClienteCreadoIntegrationEvent>(), default), Times.Once);
    }

    [Fact]
    public async Task CrearAsync_ConClienteIdDuplicado_DeberiaLanzarExcepcion()
    {
        var dto = new CrearClienteDto("CLI001", "Jose Lema", "Masculino", 35, "1023456789", "Dir", "098", "secreta1");
        _repoMock.Setup(r => r.ExisteClienteIdAsync(dto.ClienteId, default)).ReturnsAsync(true);

        var accion = async () => await _sut.CrearAsync(dto);

        await accion.Should().ThrowAsync<ClienteIdentificacionDuplicadaException>();
        _repoMock.Verify(r => r.AgregarAsync(It.IsAny<Cliente>(), default), Times.Never);
    }

    [Fact]
    public async Task EliminarAsync_ConClienteInexistente_DeberiaLanzarExcepcion()
    {
        _repoMock.Setup(r => r.ObtenerPorIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync((Cliente?)null);

        var accion = async () => await _sut.EliminarAsync(Guid.NewGuid());

        await accion.Should().ThrowAsync<ClienteNoEncontradoException>();
    }
}
