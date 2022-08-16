using AssignFPTBook.Data;
using AssignFPTBook.Models;
using AssignFPTBook.Utils;
using AssignFPTBook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            var user = _userManager.GetUserId(User);
            if (user is null)
            {
                return NotFound();
            }

            var viewModel = new ResetPasswordViewModel
            {
                Id = id,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return View();
            }

            return View();
        }

    }
}
