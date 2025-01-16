using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Book> Add(Book entity)
        {
            if (entity is not null)
                await _unitOfWork.BookRepository.AddAsync(entity);
            return entity;
        }

        public async Task<int> Delete(int entityId)
        {
            var book = await GetById(entityId);
            if (book == null)
                return -1; // Not found..!
            await _unitOfWork.BookRepository.DeleteAsync(book);
            return 1;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _unitOfWork.BookRepository.GetAll();
        }

        public async Task<Book> GetById(int entityId)
        {
            return await _unitOfWork.BookRepository.GetBookById(entityId);
        }

        public async Task<Book> Update(Book book)
        {
            if (book is not null)
                await _unitOfWork.BookRepository.UpdateAsync(book);
            return book;
        }

        public async Task<List<Book>> Search(string query)
        {
            var books = await _unitOfWork.BookRepository.Search(query);
            return books;
        }

        public async Task<bool> CanBeBorrowed(int id)
        {
            if (!await BookExists(id))
            {
                return false;
            }

            var book = await _unitOfWork.BookRepository.GetBookById(id);

            int numberOfBorrowedBooks = _unitOfWork.BookRepository.GetNumberOfBorrowedBooks(id);
            if (book == null || numberOfBorrowedBooks >= book.Quantity)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> BookExists(int id)
        {
            var book = await _unitOfWork.BookRepository.GetBookById(id);

            if (book is not null)
                return true;
            return false;
        }
    }
}
