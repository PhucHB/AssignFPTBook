using AssignFPTBook.Data;
using AssignFPTBook.Models;
using AssignFPTBook.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssignFPTBook.Controllers
{
    [Authorize(Roles = Role.ADMIN)]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public AdminController(
          UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Users()
        {
            var usersWithPermission = _userManager.GetUsersInRoleAsync(Role.USER).Result;
            return View(usersWithPermission);
        }

        [HttpGet]
        public IActionResult Stores()
        {
            var storesWithPermission = _userManager.GetUsersInRoleAsync(Role.STORE).Result;
            return View(storesWithPermission);
        }
    }
}
