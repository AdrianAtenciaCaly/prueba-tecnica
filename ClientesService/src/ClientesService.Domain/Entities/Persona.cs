namespace ClientesService.Domain.Entities;

/// <summary>
/// Clase base del dominio. Representa a cualquier persona en el sistema.
/// Cliente hereda de Persona (requisito del ejercicio).
/// La validación de invariantes vive aquí (Domain-Driven Design: la entidad se protege a sí misma),
/// no en capas superiores, para cumplir con el principio de responsabilidad única y evitar
/// que un dominio pueda existir en un estado inválido.
/// </summary>
public abstract class Persona
{
    public Guid Id { get; protected set; }
    public string Nombre { get; protected set; } = default!;
    public string Genero { get; protected set; } = default!;
    public int Edad { get; protected set; }
    public string Identificacion { get; protected set; } = default!;
    public string Direccion { get; protected set; } = default!;
    public string Telefono { get; protected set; } = default!;

    protected Persona() { } // Requerido por EF Core

    protected Persona(string nombre, string genero, int edad, string identificacion, string direccion, string telefono)
    {
        SetNombre(nombre);
        SetGenero(genero);
        SetEdad(edad);
        SetIdentificacion(identificacion);
        SetDireccion(direccion);
        SetTelefono(telefono);
    }

    public void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exceptions.ClienteDomainException("El nombre no puede estar vacío.");
        Nombre = nombre.Trim();
    }

    public void SetGenero(string genero)
    {
        if (string.IsNullOrWhiteSpace(genero))
            throw new Exceptions.ClienteDomainException("El género no puede estar vacío.");
        Genero = genero.Trim();
    }

    public void SetEdad(int edad)
    {
        if (edad < 0 || edad > 120)
            throw new Exceptions.ClienteDomainException("La edad debe estar entre 0 y 120 años.");
        Edad = edad;
    }

    public void SetIdentificacion(string identificacion)
    {
        if (string.IsNullOrWhiteSpace(identificacion))
            throw new Exceptions.ClienteDomainException("La identificación no puede estar vacía.");
        Identificacion = identificacion.Trim();
    }

    public void SetDireccion(string direccion)
    {
        if (string.IsNullOrWhiteSpace(direccion))
            throw new Exceptions.ClienteDomainException("La dirección no puede estar vacía.");
        Direccion = direccion.Trim();
    }

    public void SetTelefono(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
            throw new Exceptions.ClienteDomainException("El teléfono no puede estar vacío.");
        Telefono = telefono.Trim();
    }
}
