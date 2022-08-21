using System.ComponentModel.DataAnnotations;

namespace AssignFPTBook.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        
        public Order Order { get; set; }
        public Book Book { get; set; }

    }
}
