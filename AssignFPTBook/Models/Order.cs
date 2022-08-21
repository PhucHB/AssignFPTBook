using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignFPTBook.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public double Total { get; set; }
        public ApplicationUser User { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}
