using LibrarySystem.Data;
using LibrarySystem.Dtos;
using LibrarySystem.Models;
using LibrarySystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        public BooksController(ApplicationDbContext context, IBookService bookService, ICategoryService categoryService)
        {
            _context = context;
            _bookService = bookService;
            _categoryService = categoryService;
        }

        // GET: api/books
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookService.GetAll();
            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id, Title = b.Title, Author = b.Author, Category = new CategoryDto { Id = b.Category.Id, Name = b.Category.Name }, Quantity = b.Quantity,
                ISBN = b.ISBN, RackNumber = b.RackNumber
            }
            ).ToList();
            return Ok(bookDtos);
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Category = new CategoryDto { Id = book.Category.Id, Name = book.Category.Name },
                Quantity = book.Quantity,
                ISBN = book.ISBN,
                RackNumber = book.RackNumber
            };

            return Ok(bookDto);
        }

        // POST: api/books
        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<BookDto>> CreateBook(BookDto bookDto)
        {

            var category = await _categoryService.GetById(bookDto.Category.Id);

            if (category == null)
            {
                return BadRequest("Invalid category ID");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                CategoryId = category.Id,
                ISBN = bookDto.ISBN,
                RackNumber = bookDto.RackNumber,
                Quantity = bookDto.Quantity
            };

            await _bookService.Add(book);

            bookDto.Id = book.Id;
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> UpdateBook(int id, BookDto bookDto)
        {

            var book = await _bookService.GetById(id);
            if (book == null)
            {
                return NotFound("Book is not found!");
            }

            var category = await _categoryService.GetById(bookDto.Category.Id);

            if (category == null)
            {
                return BadRequest("Invalid category ID");
            }

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.CategoryId = category.Id;

            await _bookService.Update(book);


            return Ok("Book Updated Successfully!");
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
            {
                return NotFound("Book is not found!");
            }

            await _bookService.Delete(book.Id);
            return Ok("Book Deleted Successfully!");
        }


        // Search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query cannot be empty");
            }

            var books = await _bookService.Search(query);

            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Category = new CategoryDto { Id = b.Category.Id, Name = b.Category.Name },
                ISBN = b.ISBN, Quantity = b.Quantity, RackNumber = b.RackNumber
            }).ToList();

            return Ok(bookDtos);
        }

        [HttpGet("borrowable-books")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBorrowableBooks()
        {
            var books = await _bookService.GetAll();


            var filteredBooks = books.Where(b => _bookService.CanBeBorrowed(b.Id).Result);

            var bookDtos = filteredBooks.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                Quantity = b.Quantity,
                RackNumber = b.RackNumber,
                Category = new CategoryDto { Id = b.Category.Id, Name = b.Category.Name }
            }).ToList();

            return Ok(bookDtos);
        }

        [HttpGet("find-by-isbn")]
        public async Task<ActionResult<BookDto>> GetByISBN(string isbn)
        {
            var allBooks = await _bookService.GetAll();
            var book = allBooks.Where(b => b.ISBN == isbn).FirstOrDefault();
            if (book == null)
                return NotFound();
            return Ok(new BookDto
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Author = book.Author,
                Category = new CategoryDto
                {
                    Id = book.Category.Id,
                    Name = book.Category.Name
                },
                Quantity = book.Quantity,
                RackNumber = book.RackNumber,
                Title = book.Title
            });
        }
    }
}
