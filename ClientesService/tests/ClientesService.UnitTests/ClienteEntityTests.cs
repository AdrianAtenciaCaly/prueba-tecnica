using ClientesService.Domain.Entities;
using ClientesService.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace ClientesService.UnitTests;

/// <summary>
/// F5: Prueba unitaria de la entidad de dominio Cliente. Se prueban las invariantes del dominio
/// directamente sobre la entidad (sin mocks, sin base de datos) — pruebas rápidas y aisladas.
/// </summary>
public class ClienteEntityTests
{
    private static Cliente CrearClienteValido() =>
        Cliente.Crear(
            clienteId: "CLI001",
            nombre: "Jose Lema",
            genero: "Masculino",
            edad: 35,
            identificacion: "1023456789",
            direccion: "Otavalo sn y principal",
            telefono: "098254785",
            contrasenaHash: "hash-simulado",
            estado: true);

    [Fact]
    public void Crear_ConDatosValidos_DeberiaCrearClienteCorrectamente()
    {
        var cliente = CrearClienteValido();

        cliente.ClienteId.Should().Be("CLI001");
        cliente.Nombre.Should().Be("Jose Lema");
        cliente.Estado.Should().BeTrue();
    }

    [Fact]
    public void Crear_ConClienteIdVacio_DeberiaLanzarExcepcion()
    {
        var accion = () => Cliente.Crear("", "Jose Lema", "Masculino", 35, "1023456789", "Dir", "098", "hash");

        accion.Should().Throw<ClienteDomainException>()
            .WithMessage("*ClienteId*");
    }

    [Fact]
    public void Crear_ConEdadNegativa_DeberiaLanzarExcepcion()
    {
        var accion = () => Cliente.Crear("CLI001", "Jose Lema", "Masculino", -1, "1023456789", "Dir", "098", "hash");

        accion.Should().Throw<ClienteDomainException>()
            .WithMessage("*edad*");
    }

    [Fact]
    public void Crear_ConContrasenaVacia_DeberiaLanzarExcepcion()
    {
        var accion = () => Cliente.Crear("CLI001", "Jose Lema", "Masculino", 35, "1023456789", "Dir", "098", "");

        accion.Should().Throw<ClienteDomainException>()
            .WithMessage("*contraseña*");
    }

    [Fact]
    public void ActualizarDatos_ConDatosValidos_DeberiaActualizarCliente()
    {
        var cliente = CrearClienteValido();

        cliente.ActualizarDatos("Jose Lema Actualizado", "Masculino", 36, "Nueva dirección", "099999999");

        cliente.Nombre.Should().Be("Jose Lema Actualizado");
        cliente.Edad.Should().Be(36);
        cliente.Direccion.Should().Be("Nueva dirección");
    }

    [Fact]
    public void Desactivar_DeberiaCambiarEstadoAFalse()
    {
        var cliente = CrearClienteValido();

        cliente.Desactivar();

        cliente.Estado.Should().BeFalse();
    }
}
