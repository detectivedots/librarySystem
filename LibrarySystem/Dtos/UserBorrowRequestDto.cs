using LibrarySystem.Models;

namespace LibrarySystem.Dtos
{
    public class UserBorrowRequestDto
    {
        public int BookId { get; set; }
        public string UserName{ get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public UserBorrowRequestDto() { }

        public UserBorrowRequestDto(BorrowRequest borrowRequest)
        {
            this.BorrowDate = borrowRequest.BorrowDate;
            this.ReturnDate = borrowRequest.ReturnDate;
            this.BookId = borrowRequest.BookId;
            this.UserName = borrowRequest.User.UserName;
        }
    }
}
