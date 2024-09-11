using GestionTurnosApis.Models;

namespace GestionTurnosApis.Services
{
    public interface IAuthService
    {
        string Authenticate(string username, string password);
        Task<User> GetUserByName(string username);
    }
}
