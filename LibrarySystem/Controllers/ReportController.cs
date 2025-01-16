using LibrarySystem.Data;
using LibrarySystem.Dtos;
using LibrarySystem.Models;
using LibrarySystem.Services.Implementations;
using LibrarySystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        public ReportController(IBookService bookService, UserManager<ApplicationUser> userManager, IBorrowRequestService borrowRequestService, ICategoryService categoryService)
        {
            _bookService = bookService;
            _userManager = userManager;
            _borrowRequestService = borrowRequestService;
            _categoryService = categoryService;
        }
        [HttpGet]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<ReportDto>> GenerateReport()
        {
            var allBooks = await _bookService.GetAll();
            var allRequest = await _borrowRequestService.GetAll();
            var allBorrows = allRequest.Where(b => b.Status == "BORROWED" || b.Status == "RETURNED");
            int totalBorrowed = allRequest.Count(b => b.Status == "BORROWED");
            int totalReturned = allRequest.Count(b => b.Status == "RETURNED");
            var borrowedBooks = allBorrows.Select(b => b.Book);
            var popularCategory = borrowedBooks.GroupBy(book => book.Category)
                .OrderByDescending(group => group.Count()).Select(group => group.Key).FirstOrDefault();
            var mostBorrowedBook = allBorrows
                .GroupBy(borrow => borrow.Book)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();


            return Ok(new ReportDto
            {
                TotalBorrowed = totalBorrowed,
                TotalReturned = totalReturned,
                MostBorrowed = new BookDto
                {
                    Id = mostBorrowedBook.Id,
                    Author = mostBorrowedBook.Author,
                    Category = new CategoryDto
                    {
                        Id = mostBorrowedBook.CategoryId,
                        Name = mostBorrowedBook.Category.Name
                    },
                    ISBN = mostBorrowedBook.ISBN,
                    Quantity = mostBorrowedBook.Quantity,
                    RackNumber = mostBorrowedBook.RackNumber,
                    Title = mostBorrowedBook.Title
                }, MostPopularCategory = new CategoryDto
                {
                    Id = popularCategory.Id, Name = popularCategory.Name
                }, TotalBooks = allBooks.Count()
            });
        }
        [HttpGet("id")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<BookReportDto>> GenerateReport(int id)
        {
            var allBooks = await _bookService.GetAll();
            var book = await _bookService.GetById(id);
            if (book == null) 
            {
                return NotFound();
            }
            var allRequests = await _borrowRequestService.GetAll();
            var allBorrows = allRequests.Where(b => b.Status == "BORROWED");
            var allBookRequests = allRequests.Where(x => x.BookId == id);
            int totalBorrowed = allBookRequests.Count(b => b.Status == "BORROWED");
            int totalReturned = allBookRequests.Count(b => b.Status == "RETURNED");
            int availableCopies = book.Quantity - totalBorrowed;
            var usersWhoBorrowed = allBorrows
                .Select(borrow => borrow.UserId)
                .Distinct().ToList();
            List<string> currentlyBorrowedBy = new List<string>();
            foreach (var userId in usersWhoBorrowed)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    currentlyBorrowedBy.Add(user.UserName);
                }
            }

            return Ok(new BookReportDto
            {
                CurrentlyAvailabeCopies = availableCopies,
                CurrentlyBorrowedBy = currentlyBorrowedBy,
                TotalBorrowed = totalBorrowed,
                TotalReturned = totalReturned
            });
        }
    }
}
