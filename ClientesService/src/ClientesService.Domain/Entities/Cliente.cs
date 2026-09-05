using ClientesService.Domain.Exceptions;

namespace ClientesService.Domain.Entities;

/// <summary>
/// Cliente hereda de Persona (requisito explícito del ejercicio). Agrega ClienteId (clave única de negocio),
/// contraseña (siempre almacenada como hash, nunca en texto plano) y estado (activo/inactivo).
/// </summary>
public class Cliente : Persona
{
    public string ClienteId { get; private set; } = default!;
    public string ContrasenaHash { get; private set; } = default!;
    public bool Estado { get; private set; }

    private Cliente() { } // EF Core

    private Cliente(
        string clienteId,
        string nombre,
        string genero,
        int edad,
        string identificacion,
        string direccion,
        string telefono,
        string contrasenaHash,
        bool estado)
        : base(nombre, genero, edad, identificacion, direccion, telefono)
    {
        SetClienteId(clienteId);
        ContrasenaHash = contrasenaHash;
        Estado = estado;
    }

    /// <summary>
    /// Factory method: punto único de creación válida de un Cliente (Factory pattern + encapsulación).
    /// Recibe la contraseña ya hasheada; el hashing es una preocupación de infraestructura/aplicación, no de dominio.
    /// </summary>
    public static Cliente Crear(
        string clienteId,
        string nombre,
        string genero,
        int edad,
        string identificacion,
        string direccion,
        string telefono,
        string contrasenaHash,
        bool estado = true)
    {
        if (string.IsNullOrWhiteSpace(contrasenaHash))
            throw new ClienteDomainException("La contraseña no puede estar vacía.");

        return new Cliente(clienteId, nombre, genero, edad, identificacion, direccion, telefono, contrasenaHash, estado);
    }

    public void SetClienteId(string clienteId)
    {
        if (string.IsNullOrWhiteSpace(clienteId))
            throw new ClienteDomainException("El identificador de cliente (ClienteId) no puede estar vacío.");
        ClienteId = clienteId.Trim();
    }

    public void ActualizarDatos(string nombre, string genero, int edad, string direccion, string telefono)
    {
        SetNombre(nombre);
        SetGenero(genero);
        SetEdad(edad);
        SetDireccion(direccion);
        SetTelefono(telefono);
    }

    public void CambiarContrasena(string nuevaContrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(nuevaContrasenaHash))
            throw new ClienteDomainException("La contraseña no puede estar vacía.");
        ContrasenaHash = nuevaContrasenaHash;
    }

    public void Activar() => Estado = true;

    public void Desactivar() => Estado = false;
}
