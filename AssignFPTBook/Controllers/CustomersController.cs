using AssignFPTBook.Data;
using AssignFPTBook.Models;
using AssignFPTBook.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignFPTBook.Controllers
{
    [Authorize(Roles = Role.USER)]
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // test 
        public CustomersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(string search)
        {

            if (!string.IsNullOrWhiteSpace(search))
            {
                var result = _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.Category.Description.ToLower().Equals(search))
                    .ToList();
                
                return View(result);
            }
            
            IEnumerable<Book> customers = _context.Books
                .Include(b => b.Category)
                .ToList();

            
            return View(customers);
        }
        public IActionResult Details(int id)
        {
            var bookInDb = _context.Books.Include(b => b.Category).SingleOrDefault(b => b.Id == id );
            IEnumerable<Book> customers = _context.Books
                .Include(b => b.Category)
                .ToList();
            if (bookInDb is null)
            {
                return NotFound();
            }
            string imageBase64Data = Convert.ToBase64String(bookInDb.ImageData);

            string image = string.Format("data:image/jpg;base64, {0}", imageBase64Data);
            ViewBag.ImageData = image;
            return View(bookInDb);
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {

            var profileUser = _userManager.GetUserAsync(User).Result;
            return View(profileUser);

        }
    }
}
