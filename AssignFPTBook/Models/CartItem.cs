

namespace AssignFPTBook.Models
{
    public class CartItem
    {
        public Book book { get; set;  }
        public ApplicationUser user { get; set; }
        public int Quantity { get; set; }
        
  }
}
