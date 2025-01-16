using LibrarySystem.Models;

namespace LibrarySystem.Services.Interfaces
{
    public interface IBookService
    {
        public Task<Book> Add(Book entity);
        public Task<int> Delete(int entityId);

        public Task<List<Book>> GetAll();
        public Task<Book> GetById(int entityId);
        public Task<Book> Update(Book book);

        public Task<List<Book>> Search(string query);
        public Task<bool> CanBeBorrowed(int id);
    }
}
