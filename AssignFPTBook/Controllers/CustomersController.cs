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
        public IActionResult Index(string category)
        {
            
            if (!string.IsNullOrWhiteSpace(category))
            {
                var result = _context.Books.Include(b => b.Category).Where(b => b.Category.Description.Equals(category)).ToList();
                
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
    }
}
