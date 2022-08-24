using AssignFPTBook.Data;

using AssignFPTBook.Models;
using AssignFPTBook.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AssignFPTBook.Controllers
{
    public class CartController : Controller
    {
        public const string CARTKEY = "cart";
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private string Message ="Order success please check your order ";
        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        public IActionResult Index()
        {
            return View(GetCartItems());
        }

        public Book getBook(int id)
        {
            var _book = _context.Books.Find(id);
            return _book;
        }
        
        public IActionResult AddCart(int id)
        {
            var currentId = _userManager.GetUserId(User);
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            var cart = GetCartItems();
            var cartitem = cart.Find(b => b.book.Id == id);



            if (book == null)
            { return NotFound("Do not have this book!"); }


            if (cartitem != null)
            {
                cartitem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem() { Quantity = 1, book = book });
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
            
        }

        public IActionResult RemoveCart(int id)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.book.Id == id);
            if (cartitem != null)
            {

                cart.Remove(cartitem);
            }

            SaveCartSession(cart);

            return RedirectToAction(nameof(Index));
        }
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int id, [FromForm] int quantity)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.book.Id == id);
            if (cartitem != null)
            {
                cartitem.Quantity = quantity;
            }
            SaveCartSession(cart);
            return Ok();
        }


        public async Task< IActionResult> CheckOut()
        {
            
            List<CartItem> myDetailsInCart = GetCartItems();
            using (var ttransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var CurentId = _userManager.GetUserId(User);
                    Order myorder = new Order();
                    {
                        myorder.UserID = CurentId;
                        myorder.Total = myDetailsInCart.Select(c => c.book.Price).Aggregate((c1, c2) => c1 + c2);
                    };
                    
                    _context.Add(myorder);
                    await _context.SaveChangesAsync();

                    foreach (var item in myDetailsInCart)
                    {
                        OrderDetail detail = new OrderDetail()
                        {
                            OrderId = myorder.Id,
                            BookId = item.book.Id,
                            Quantity = 1,
                            shopID = item.book.UserId
                        };
                        
                        _context.Add(detail);
                        await _context.SaveChangesAsync();
                    }
                    
                    //_context.SaveChanges();
                    //RemoveCart(detail.BookId);
                    ClearCart();

            }
                catch (DbUpdateException ex)
            {

                Console.WriteLine("Error occurred in Checkout" + ex);

            }
        }
            
                return RedirectToAction("Index");
        }


       
        //public async Task<IActionResult> Checkout(string returnUrl = null)
        //{
        //    int quantity = 1;
        //    FPTBookUser thisUser = await _userManager.GetUserAsync(HttpContext.User);
        //    List<Cart> myDetailsInCart = await _context.Carts
        //        .Where(c => c.UId == thisUser.Id)
        //        .Include(c => c.Book)
        //        .ToListAsync();
        //    var Cart = _context.Carts.Include(c => c.Book).Where(b => b.UId == thisUser.Id).FirstOrDefault();

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            //Step 1: create an order
        //            Order myOrder = new Order();
        //            myOrder.Uid = thisUser.Id;
        //            myOrder.OrderDate = DateTime.Now;
        //            myOrder.Total = myDetailsInCart.Select(c => c.Price)
        //       .Aggregate((c1, c2) => c1 + c2);

        //            _context.Add(myOrder);
        //            await _context.SaveChangesAsync();

        //            //Step 2: insert all order details by var "myDetailsInCart"
        //            foreach (var item in myDetailsInCart)
        //            {
        //                OrderDetail detail = new OrderDetail()
        //                {
        //                    OderID = myOrder.Id,
        //                    BookIisbn = item.BookIsbn,
        //                    Quantity = item.Quantity
        //                };
        //                _context.Add(detail);
        //            }
        //            await _context.SaveChangesAsync();

        //            //Step 3: empty/delete the cart we just done for thisUser
        //            _context.Carts.RemoveRange(myDetailsInCart);
        //            await _context.SaveChangesAsync();

        //            transaction.Comm it();
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            transaction.Rollback();
        //            Console.WriteLine("Error occurred in Checkout" + ex);
        //        }
        //    }
        //    return RedirectToAction("Email");
        //}
    }   
}
