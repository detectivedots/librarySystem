namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; } 
        ICategoryRepository CategoryRepository { get; }
        IBorrowRequestRepository BorrowRequestRepository { get; }   

    }
}
