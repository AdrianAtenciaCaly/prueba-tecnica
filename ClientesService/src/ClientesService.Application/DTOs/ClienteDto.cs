namespace ClientesService.Application.DTOs;

public record ClienteDto(
    Guid Id,
    string ClienteId,
    string Nombre,
    string Genero,
    int Edad,
    string Identificacion,
    string Direccion,
    string Telefono,
    bool Estado);
