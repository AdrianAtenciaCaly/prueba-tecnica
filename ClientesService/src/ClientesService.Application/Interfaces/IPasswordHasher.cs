namespace ClientesService.Application.Interfaces;

/// <summary>Abstracción para el hashing de contraseñas (Single Responsibility + testabilidad).</summary>
public interface IPasswordHasher
{
    string Hash(string contrasenaPlana);
    bool Verificar(string contrasenaPlana, string hash);
}
