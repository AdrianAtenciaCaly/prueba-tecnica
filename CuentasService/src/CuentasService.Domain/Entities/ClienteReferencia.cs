namespace CuentasService.Domain.Entities;

/// <summary>
/// Modelo de lectura local ("read model") mantenido dentro de CuentasService, actualizado de forma
/// asíncrona mediante los eventos de integración que publica ClientesService (ClienteCreado/Actualizado/Eliminado).
/// Esto es lo que le permite a CuentasService validar la existencia/estado de un cliente al abrir una cuenta
/// SIN hacer una llamada síncrona HTTP al otro microservicio (patrón de comunicación asíncrona desacoplada).
/// </summary>
public class ClienteReferencia
{
    public string ClienteId { get; private set; } = default!;
    public string Nombre { get; private set; } = default!;
    public bool Estado { get; private set; }
    public DateTime ActualizadoEn { get; private set; }

    private ClienteReferencia() { } // EF Core

    public ClienteReferencia(string clienteId, string nombre, bool estado)
    {
        ClienteId = clienteId;
        Nombre = nombre;
        Estado = estado;
        ActualizadoEn = DateTime.UtcNow;
    }

    public void Actualizar(string nombre, bool estado)
    {
        Nombre = nombre;
        Estado = estado;
        ActualizadoEn = DateTime.UtcNow;
    }

    public void MarcarEliminado() => Estado = false;
}
