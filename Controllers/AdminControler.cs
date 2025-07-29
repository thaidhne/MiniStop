using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniStop.Common.Security;
using MiniStop.Database;
using MiniStop.Interface;
using MiniStop.Models;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPassword _password;

    public AdminController(ApplicationDbContext context, IPassword password)
    {
        _context = context;
        _password = password;
    }
    public IActionResult Admin()
    {
        var users = from u in _context.Users
                    join r in _context.Roles on u.RoleId equals r.RoleId
                    select new AdminViewModel
                    {
                        Usrid = u.Usrid,
                        Username = u.Username,
                        FullName = u.FullName,
                        PhoneNumber = u.PhoneNumber,
                        Email = u.Email,
                        IsActive = u.IsActive,
                        RoleName = r.Name,
                    };

        return PartialView("Admin", users.ToList());
    }
    public IActionResult Product()
    {
        return PartialView("Product");
    }

    [HttpGet]
    public IActionResult Index()
    {

        bool hasChangedPassword = _context.Users.FirstOrDefault(u => u.Usrid == HttpContext.Session.GetInt32("UserId"))?.IsPassWordDefault ?? false;

        if (hasChangedPassword)
        {
            HttpContext.Session.SetString("ShowResetPasswordPopup", "true"); 
        }

        
        return View();
    }

    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordViewModel model)
    {

        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.FirstOrDefault(u => u.Usrid == userId);

            if (user != null)
            {
                user.PasswordHash = _password.HashPassword(model.NewPassword);
                user.IsPassWordDefault = false;
                _context.SaveChanges();
                TempData["ShowResetPasswordPopup"] = null;
                HttpContext.Session.Remove("ShowResetPasswordPopup");
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(string.Empty, "User not found.");


            return View(model);

        }
    }

    public IActionResult Details(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Usrid == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
        
    }

    public IActionResult btnSave(int id, User User)
    {
        var user = _context.Users.FirstOrDefault(u => u.Usrid == User.Usrid);

        if (user == null)
        {
            
            return NotFound();
        }

        // Cập nhật thông tin từ model gửi lên
        user.FullName = User.FullName;
        user.Email = User.Email;
        user.PhoneNumber = User.PhoneNumber;
 
        _context.SaveChanges();

        // Chuyển hướng người dùng về trang chi tiết nhân viên hoặc danh sách
        return RedirectToAction("Details", new { id = user.Usrid });

    }

    public IActionResult AddNew()
    {
        return View();
    }
}
