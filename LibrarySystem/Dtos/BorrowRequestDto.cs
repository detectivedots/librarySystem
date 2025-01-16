using LibrarySystem.Models;

namespace LibrarySystem.Dtos
{
    public class BorrowRequestDto
    {
        public int BookId { get; set; }
        public string UserName { get; set; }

        public string Status { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public BorrowRequestDto() { }

        public BorrowRequestDto(BorrowRequest borrowRequest) 
        {
            this.BorrowDate = borrowRequest.BorrowDate;
            this.ReturnDate = borrowRequest.ReturnDate;
            this.Status = borrowRequest.Status;
            this.BookId = borrowRequest.BookId;
            this.UserName = borrowRequest.User.UserName;
        }
    }
}
