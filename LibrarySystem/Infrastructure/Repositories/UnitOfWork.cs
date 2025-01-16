using LibrarySystem.Infrastructure.Interfaces;

namespace LibrarySystem.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        public IBorrowRequestRepository BorrowRequestRepository { get;}
        public ICategoryRepository CategoryRepository { get; }
        public IBookRepository BookRepository { get; }


        public UnitOfWork(IBookRepository bookRepository, ICategoryRepository categoryRepository, IBorrowRequestRepository borrowRequestRepository)
        {
            this.BookRepository = bookRepository;
            this.CategoryRepository = categoryRepository;
            this.BorrowRequestRepository = borrowRequestRepository;
        }
    }
}
