using MiniStop.Models;
using System.Threading.Tasks;

namespace MiniStop.Services.Interface
{
    public interface IAccountService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> ResetPasswordAsync(int userId, string newPassword);
        Task<bool> UpdateUserInfoAsync(User user);

        Task<User> GetUserByIdAsync(int usrid);
        Task<User> GetUserByUserAsync(string username);

        Task<List<AdminViewModel>> GetAllUsersAsync();

        Task<User> AddUserInfoAsync(User user);

    }
}
