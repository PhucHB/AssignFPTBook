using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AssignFPTBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public List<Book> Books { get; set; }
        public List<ContactAdmin> contactAdmins { get; set; }
    }
}
