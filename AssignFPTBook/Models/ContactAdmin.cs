using AssignFPTBook.Enums;
using System;

namespace AssignFPTBook.Models
{
    public class ContactAdmin
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string DecriptionContact { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ContactStatus Status { get; set; } = ContactStatus.pending;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
    }
}
