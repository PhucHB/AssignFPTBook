using Microsoft.AspNetCore.Mvc;

namespace AssignFPTBook.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
