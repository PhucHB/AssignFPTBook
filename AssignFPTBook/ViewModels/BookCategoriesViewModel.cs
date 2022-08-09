using AssignFPTBook.Models;
using System.Collections.Generic;

namespace AssignFPTBook.ViewModels
{
    public class BookCategoriesViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
