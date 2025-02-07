using VillustraTask.Api.Models;

namespace VillustraTask.Api.Interfaces
{
    public interface IUserRepository
    {
        Task<Userlogin> GetUserByIdAsync(string userId);
        Task<int> InsertUserAsync(Userlogin user);
        Task<Userlogin?> AuthenticateUserAsync(string userId, string password);
    }
}
