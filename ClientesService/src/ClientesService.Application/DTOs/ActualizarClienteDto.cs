namespace ClientesService.Application.DTOs;

public record ActualizarClienteDto(
    string Nombre,
    string Genero,
    int Edad,
    string Direccion,
    string Telefono,
    bool Estado);
