using LibrarySystem.Models;

namespace LibrarySystem.Dtos
{
    public class BookReportDto
    {
        public int TotalBorrowed {  get; set; } 
        public int TotalReturned { get; set; }
        public List<string> CurrentlyBorrowedBy { get; set; }
        public int CurrentlyAvailabeCopies {  get; set; }
    }
}
