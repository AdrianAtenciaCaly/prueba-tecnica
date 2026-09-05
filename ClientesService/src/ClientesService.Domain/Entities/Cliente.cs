using ClientesService.Domain.Exceptions;

namespace ClientesService.Domain.Entities;

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
