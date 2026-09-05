namespace ClientesService.Application.DTOs;

/// <summary>DTO de entrada para F1 - actualización de Cliente.</summary>
public record ActualizarClienteDto(
    string Nombre,
    string Genero,
    int Edad,
    string Direccion,
    string Telefono,
    bool Estado);
