using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignFPTBook.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "You need to add Title ...")]
        public string Title { get; set; }
        public string Author { get; set; }
        [Required(ErrorMessage = "You need to add Description ...")]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public double Price { get; set; }
        public byte[] ImageData { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }

    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Description cannot be null ...")]
        [StringLength(255)]
        public String Description { get; set; }
        public List<Book> Books { get; set; }


    }
}
