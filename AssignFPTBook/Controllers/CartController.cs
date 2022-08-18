using AssignFPTBook.Data;

using AssignFPTBook.Models;
using AssignFPTBook.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AssignFPTBook.Controllers
{
    public class CartController : Controller
    {
        public const string CARTKEY = "cart"; 
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // test 
        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Lấy cart từ Session (danh sách CartItem)
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
        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        //-------------------------Controler---------------------
        public IActionResult Index()
        {
            //var cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart ");
            //ViewBag.cart = cart;
            //ViewBag.total = cart.Sum(item => item.book.Price * item.Quantity);
            return View(GetCartItems());
        }
        //[Route("addcart/{BookId:int}" , Name ="addcart")]
        public IActionResult AddToCart([FromRoute]
                                       int BookId)
        {
            var book = _context.Books.Where(b=>b.Id == BookId).FirstOrDefault();
            if (BookId == null) { return NotFound("k có sản phẩm này"); }
                
            var cart = GetCartItems();
            var cartitem = cart.Find(b=>b.book.Id == BookId);
            if (cartitem != null)
            {
                cartitem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem() { Quantity = 1 ,book=book});
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
            //Book book =new Book();
            //if(SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart ") == null){
            //    List<CartItemViewModel> cart = new List<CartItemViewModel>();
            //    cart.Add(new CartItemViewModel { book = book, Quantity = 1 });
            //    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            //}
            //else
            //{
            //    List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson< List<CartItemViewModel>>(HttpContext.Session, "cart ");
            //    int index = isExist(id);
            //    if(index != -1)
            //    {
            //        cart[index].Quantity++;
            //    }
            //    else
            //    {
            //        cart.Add(new CartItemViewModel { book = book, Quantity = 1 });
            //    }
            //    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            //}
            //return RedirectToAction("Index");
        }
        [Route("/removecart/{Bookid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int Bookid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.book.Id == Bookid);
            if (cartitem != null)
            {
                
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            
            return RedirectToAction(nameof(Index));
        }
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int Bookid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.book.Id == Bookid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.Quantity = quantity;
            }
            SaveCartSession(cart);
            return Ok();
        }

        [Route("/checkout")]
        public IActionResult CheckOut()
        {
            // Xử lý khi đặt hàng
            return View();
        }
    }   
}
