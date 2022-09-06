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

    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = Role.ADMIN)]
        
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Category> category = _context.Categories.ToList();
            return View(category);
        }
        [Authorize(Roles = Role.STORE)]
        public IActionResult Indexc()
        {
            IEnumerable<Category> category = _context.Categories.ToList();
            return View(category);
        }


        [Authorize(Roles = Role.STORE)]
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [Authorize(Roles = Role.STORE)]
        [HttpPost]
        public IActionResult Create(Category category)
        {
            
            var newCategory = new Category
            {
                Description = category.Description,
                
            };
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Indexc");
        }

        [Authorize(Roles = Role.ADMIN)]
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
        [Authorize(Roles = Role.ADMIN)]
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            var CInDb = _context.Categories.SingleOrDefault(c => c.Id == category.Id);
            if (CInDb is null)
            {
                return BadRequest();
            }
            CInDb.Description = category.Description;
            CInDb.Status = category.Status;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = Role.ADMIN)]
        [HttpGet]
        public IActionResult CreateAd()
        {

            return View();
        }
        [Authorize(Roles = Role.ADMIN)]
        [HttpPost]
        public IActionResult CreateAd(Category category)
        {

            var newCategory = new Category
            {
                Description = category.Description,
                Status = Enums.ContactStatus.Accept,

            };
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public  IActionResult Delete(int id)
        {
            var CateInDb = _context.Categories.SingleOrDefault(b => b.Id == id );
            if (CateInDb is null)
            {
                return NotFound();
            }
            _context.Categories.Remove(CateInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
