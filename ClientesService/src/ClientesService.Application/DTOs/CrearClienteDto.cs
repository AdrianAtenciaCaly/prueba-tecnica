namespace ClientesService.Application.DTOs;
public record CrearClienteDto(
    string ClienteId,
    string Nombre,
    string Genero,
    int Edad,
    string Identificacion,
    string Direccion,
    string Telefono,
    string Contrasena,
    bool Estado = true);
