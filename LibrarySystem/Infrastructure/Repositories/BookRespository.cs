using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LibrarySystem.Infrastructure.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DbSet<Book> _books;
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _books = context.Set<Book>();
            _context = context;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _books.Include(u => u.Category).ToListAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _books.Include(b => b.Category).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Book>> Search(string query)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
                .Include(b => b.Category)
                .ToListAsync();
        }

        public int GetNumberOfBorrowedBooks(int bookId)
        {
            return _context.BorrowRequests.Count(b => b.BookId == bookId && b.Status == "BORROWED");
        }

    }
}
