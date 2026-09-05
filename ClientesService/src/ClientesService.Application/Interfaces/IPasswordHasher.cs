namespace ClientesService.Application.Interfaces;
public interface IPasswordHasher
{
    string Hash(string contrasenaPlana);
    bool Verificar(string contrasenaPlana, string hash);
}
