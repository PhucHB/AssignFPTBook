using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignFPTBook.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public double Total { get; set; }
        public ApplicationUser User { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }

    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Book Book { get; set; }

    }
}
