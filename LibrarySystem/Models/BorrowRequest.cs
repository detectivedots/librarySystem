namespace LibrarySystem.Models
{
    public class BorrowRequest
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }  
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
    }
}
