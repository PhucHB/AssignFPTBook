using AssignFPTBook.Data;
using AssignFPTBook.Models;
using AssignFPTBook.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AssignFPTBook.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // test 
        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var order = _context.Orders 
                .Where(o => o.User.Id == currentUserId).Include(s => s.OrderDetails)
                .ToList();
            return View(order);
        }
        public IActionResult Detail(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var ordeDdetails = _context.OrderDetails.Where(od => od.OrderId == id).Include(s => s.Book).Include(s => s.Order).Include(s => s.Book.User);
            return View(ordeDdetails);
        }
        public IActionResult OrderStore(string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var result = _context.OrderDetails
                    .Include(b => b.Order.User)
                    .Where(b => b.Order.User.Email.ToLower().Equals(search))
                    .ToList();

                return View(result);
            }

            var currentUserId = _userManager.GetUserId(User);
            var order = _context.OrderDetails
                .Where(o => o.shopID == currentUserId).Include(s => s.Order).Include(s => s.Order.User)
                .ToList();
            return View(order);
        }
        public IActionResult DetailOrderStore(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var ordeDdetails = _context.OrderDetails.Where(od => od.Id == id).Include(s => s.Book).Include(s => s.Order).Include(s => s.Book.User);
            return View(ordeDdetails);
        }


        //public async Task<IActionResult> HistoryOrderOfSeller()
        //{
        //    FPTBookUser thisUser = await _userManager.GetUserAsync(HttpContext.User);
        //    List<Store> _store = await _context.Store.Where(s => s.UId == thisUser.Id).ToListAsync();
        //    foreach (var _itemstore in _store)
        //    {
        //        List<Book> book = await _context.Book.Where(s => s.StoreId == _itemstore.id).ToListAsync();
        //        foreach (var _itembook in book)
        //        {
        //            List<OrderDetail> orderDetails = await _context.OrderDetail.Where(o => o.BookIisbn == _itembook.Isbn).ToListAsync();
        //            foreach (var _itemorderdetail in orderDetails)
        //            {
        //                var _orderUser = _context.Orders.Where(o => o.Id == _itemorderdetail.OderID).Include(o => o.User);
        //                List<Order> _orderdetailuser = await _orderUser.ToListAsync();
        //                return View(_orderdetailuser);
        //            }
        //        }
        //    }
        //    return View();
    }

    }

