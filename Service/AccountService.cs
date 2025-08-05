using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniStop.Database;
using MiniStop.Models;
using MiniStop.Service.Interface;
using MiniStop.Services.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace MiniStop.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPassword _password;

        public AccountService(ApplicationDbContext context, IPassword password)
        {
            _context = context;
            _password = password;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
           
            var passwordHash = _password.HashPassword(password);
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.Username == username);
            return user != null;

        }

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Usrid == userId);
            if (user != null)
            {
                user.PasswordHash = _password.HashPassword(newPassword);
                user.IsPassWordDefault = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> GetUserByUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username) ?? new User();

        }

        public async Task<User> GetUserByIdAsync(int Usrid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Usrid == Usrid) ?? new User();

        }

        public async Task<List<AdminViewModel>> GetAllUsersAsync()
        {
            var Users = await (from u in _context.Users
                                join r in _context.Roles on u.RoleId equals r.RoleId
                                select new AdminViewModel
                                {
                                    Usrid = u.Usrid,
                                    Username = u.Username,
                                    FullName = u.FullName,
                                    PhoneNumber = u.PhoneNumber,
                                    Email = u.Email,
                                    IsActive = u.IsActive,
                                    RoleName = r.Name
                                }).ToListAsync();
            return Users;
        }

        public async Task<bool> UpdateUserInfoAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Usrid == user.Usrid);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> AddUserInfoAsync(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return user;
            }
            var newUser = new User
            {
                Usrid = user.Usrid,
                Username = user.Username,
                PasswordHash = _password.GenFirstPassword(8),
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleId = 100, 
                AvatarUrl = string.Empty,
                IsActive = true,
                CreatedAt = DateTime.Now,
                LastLogin = null,
                IsPassWordDefault = true
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
    }
}
