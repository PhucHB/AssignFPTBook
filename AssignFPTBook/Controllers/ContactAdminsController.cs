using AssignFPTBook.Data;
using AssignFPTBook.Enums;
using AssignFPTBook.Models;
using AssignFPTBook.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AssignFPTBook.Controllers
{
    public class ContactAdminsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ContactAdminsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        [Authorize(Roles = Role.ADMIN)]
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<ContactAdmin> contacts = _context.contactAdmins.ToList();
            return View(contacts);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var CtInDb = _context.contactAdmins.SingleOrDefault(ct => ct.Id == id);
            if (CtInDb is null)
            {
                return NotFound();
            }

            return View(CtInDb);
        }
        [Authorize(Roles = Role.ADMIN)]
        [HttpPost]
         public IActionResult Edit( ContactAdmin contactAdmin)
        {
            var CtInDb = _context.contactAdmins.SingleOrDefault(ct => ct.Id == contactAdmin.Id);
            if (CtInDb is null)
            {
                return BadRequest();
            }
            CtInDb.Status = contactAdmin.Status;
            _context.SaveChanges();
            return RedirectToAction("edit");
        }
        [Authorize(Roles = Role.STORE)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = Role.STORE)]
        [HttpPost]
        public IActionResult Create(ContactAdmin contactAdmin)
        {
            var currentUserId = _userManager.GetUserId(User);
            var newContact = new ContactAdmin
            {
                Name = contactAdmin.Name,
                DecriptionContact = contactAdmin.DecriptionContact,
                UserId = currentUserId
            };
            _context.Add(newContact);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CustomerView()
        {
            var currentUserId = _userManager.GetUserId(User);
            IEnumerable<ContactAdmin> CtAdmin = _context.contactAdmins
                .Where(b => b.UserId == currentUserId)
                .ToList();

            return View(CtAdmin);
        }


        }
}
