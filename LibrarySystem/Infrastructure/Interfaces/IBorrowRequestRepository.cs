using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IBorrowRequestRepository : IGenericRepository<BorrowRequest>
    {
        public Task<List<BorrowRequest>> GetAll();
    }
}
