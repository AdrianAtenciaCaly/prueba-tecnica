using ClientesService.Application.Interfaces;

namespace ClientesService.Infrastructure.Security;

/// <summary>Implementación concreta de IPasswordHasher usando BCrypt (salting + hashing seguro, nunca texto plano).</summary>
public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string contrasenaPlana) => BCrypt.Net.BCrypt.HashPassword(contrasenaPlana);

    public bool Verificar(string contrasenaPlana, string hash) => BCrypt.Net.BCrypt.Verify(contrasenaPlana, hash);
}
