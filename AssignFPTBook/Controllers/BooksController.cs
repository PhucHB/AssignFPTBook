using AssignFPTBook.Data;
using AssignFPTBook.Models;
using AssignFPTBook.Utils;
using AssignFPTBook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AssignFPTBook.Controllers
{
    [Authorize(Roles = Role.STORE)]
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // test 
        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(string category)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (!string.IsNullOrWhiteSpace(category))
            {
                var result = _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.Category.Description.Equals(category) && b.UserId == currentUserId).ToList();
                return View(result);
            }
            IEnumerable<Book> books = _context.Books
                .Include(b => b.Category)
                .Where(b => b.UserId == currentUserId)
                .ToList();

            return View(books);
            //if (!string.IsNullOrWhiteSpace(category))
            //{
            //    var result = _context.Books.Include(b => b.Category).Where(b => b.Category.Description.Equals(category)).ToList();
            //    return View(result);
            //}
            //IEnumerable<Book> books = _context.Books
            //    .Include(b => b.Category)
            //    .ToList();

            //return View(books);


        }

        [HttpGet]
        public IActionResult Create()
        {
            
            var viewModel = new BookCategoriesViewModel()
            {
                Categories = _context.Categories.ToList()


            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(BookCategoriesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = new BookCategoriesViewModel
                {
                    Categories = _context.Categories.ToList()
                };
                return View();
            }
            var currentUserId = _userManager.GetUserId(User);
            var newBook = new Book
            {

                Title = viewModel.Book.Title,
                Description = viewModel.Book.Description,
                Author = viewModel.Book.Author,
                Price = viewModel.Book.Price,
                CategoryId = viewModel.Book.CategoryId,
                UserId = currentUserId
            };
            _context.Add(newBook);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id && b.UserId == currentUserId);
            if (bookInDb is null)
            {
                return NotFound();
            }
            _context.Books.Remove(bookInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var bookInDb = _context.Books.Include(b => b.Category).SingleOrDefault(b => b.Id == id && b.UserId == currentUserId);
            IEnumerable<Book> books = _context.Books
                .Include(b => b.Category)
                .ToList();
            if (bookInDb is null)
            {
                return NotFound();
            }
            return View(bookInDb);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            var currentUserId = _userManager.GetUserId(User);
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id && b.UserId == currentUserId);
            if (bookInDb is null)
            {
                return NotFound();
            }

            var viewModel = new BookCategoriesViewModel
            {
                Book = bookInDb,
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(BookCategoriesViewModel viewModel)
        {
            var currentUserId = _userManager.GetUserId(User);
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == viewModel.Book.Id && b.UserId == currentUserId);
            if (bookInDb is null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                viewModel = new BookCategoriesViewModel
                {
                    Book = viewModel.Book,
                    Categories = viewModel.Categories.ToList()
                };
            }
            bookInDb.Title = viewModel.Book.Title;
            bookInDb.CategoryId = viewModel.Book.CategoryId;
            bookInDb.Description = viewModel.Book.Description;
            bookInDb.Author = viewModel.Book.Author;
            bookInDb.Price = viewModel.Book.Price;
            bookInDb.ImageData = viewModel.Book.ImageData;
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
