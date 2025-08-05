using Microsoft.AspNetCore.Mvc;
using MiniStop.Models;
using MiniStop.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

public class AdminController : Controller
{
    private readonly IAccountService _accountService;

    public AdminController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            bool isAuthenticated = await _accountService.AuthenticateAsync(model.Username, model.Password);
            if (isAuthenticated)
            {
                var user = await _accountService.GetUserByUserAsync(model.Username); 
                HttpContext.Session.SetInt32("UserId", user.Usrid);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);

                if (user.IsPassWordDefault)
                {
                    TempData["ShowResetPasswordPopup"] = true;
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(string.Empty, "Invalid account or password");
        }

        return View(model);
    }

    public Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        return Task.FromResult<IActionResult>(RedirectToAction("Login", "Account"));
    }

    public async Task<IActionResult> UserAccount()
    {
        var users = await _accountService.GetAllUsersAsync(); 
        return PartialView("Account/UserAccount", users);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        bool hasChangedPassword = (await _accountService.GetUserByIdAsync(HttpContext.Session.GetInt32("UserId") ?? 0))?.IsPassWordDefault ?? false;

        if (hasChangedPassword)
        {
            HttpContext.Session.SetString("ShowResetPasswordPopup", "true");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        bool isPasswordChanged = await _accountService.ResetPasswordAsync(userId ?? 0, model.NewPassword);

        if (isPasswordChanged)
        {
            TempData["ShowResetPasswordPopup"] = null;
            HttpContext.Session.Remove("ShowResetPasswordPopup");
            return RedirectToAction("Index", "Admin");
        }

        ModelState.AddModelError(string.Empty, "User not found.");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAccount(int id)
    {
        var user = await _accountService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View("Account/DetailsAccount", user);
    }

    [HttpPost]
    public async Task<IActionResult> btnSave(int id, User user)
    {
        var isUpdated = await _accountService.UpdateUserInfoAsync(user);
        if (!isUpdated)
        {
            return NotFound();
        }
        return RedirectToAction("DetailsAccount","Admin",new { id = user.Usrid });
    }
    
    
    
    [HttpGet]
    public IActionResult AddAccount()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddAccount(User user)
    {
        var newUser = await _accountService.AddUserInfoAsync(user);
        
        return View(newUser);
    }
}
