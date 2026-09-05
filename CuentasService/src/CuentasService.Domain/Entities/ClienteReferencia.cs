namespace CuentasService.Domain.Entities;

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
