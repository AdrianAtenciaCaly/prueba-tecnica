namespace ClientesService.Application.DTOs;

/// <summary>DTO de salida. Nunca expone la contraseña/hash (principio de menor privilegio en el contrato de API).</summary>
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
