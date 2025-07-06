using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniStop.Database;
using MiniStop.Models;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
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

        return View(users.ToList());
    }
}
