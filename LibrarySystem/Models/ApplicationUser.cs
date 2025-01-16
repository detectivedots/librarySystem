using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsApproved {  get; set; }
        public virtual ICollection<BorrowRequest> BorrowRequests { get; set; }
    }
}
