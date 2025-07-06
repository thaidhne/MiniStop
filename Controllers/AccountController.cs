using Microsoft.AspNetCore.Mvc;
using MiniStop.Database;
using MiniStop.Models;
using System.Security.Cryptography;
using System.Text;

namespace MiniStop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = HashPassword(model.Password),
                    FullName = model.Username, 
                    Email = model.Email, 
                    PhoneNumber = model.PhoneNumber, 
                    RoleId = 100, 
                    AvatarUrl = string.Empty, 
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastLogin = null 
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var passwordHash = HashPassword(model.Password);
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == passwordHash);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Usrid);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetInt32("RoleId", user.RoleId);
                    // Login success
                    return RedirectToAction("Admin", "Admin");
                }


                ModelState.AddModelError(string.Empty, "Invalid account or password");
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Account");
        }
    }
}

