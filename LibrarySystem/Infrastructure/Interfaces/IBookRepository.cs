using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        public Task<List<Book>> GetAll();
        public Task<Book> GetBookById(int id);
        public Task<List<Book>> Search(string query);
        public int GetNumberOfBorrowedBooks(int bookId);
    }
}
