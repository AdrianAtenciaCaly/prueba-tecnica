namespace ClientesService.Application.DTOs;

/// <summary>DTO de entrada para F1 - creación de Cliente.</summary>
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
