using AssignFPTBook.Data;
using AssignFPTBook.Models;
using AssignFPTBook.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AssignFPTBook.Controllers
{
    [Authorize(Roles = Role.ADMIN)]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Category> category = _context.Categories.ToList();
            return View(category);
        }
       
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        
        [HttpPost]
        public IActionResult Create(Category category)
        {
            
            var newCategory = new Category
            {
                Description = category.Description,
                
            };
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
           
            var CInDb = _context.Categories.SingleOrDefault(c => c.Id == id);
            if (CInDb is null)
            {
                return NotFound();
            }

            return View(CInDb);
        }
       
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            var CInDb = _context.Categories.SingleOrDefault(c => c.Id == category.Id);
            if (CInDb is null)
            {
                return BadRequest();
            }
            CInDb.Description = category.Description;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
