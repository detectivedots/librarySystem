using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.Repositories;
using LibrarySystem.Models;

namespace LibrarySystem.Services.Interfaces
{
    public interface IBorrowRequestService
    {
        public Task<BorrowRequest> Add(BorrowRequest entity);

        public Task<int> Delete(int entityId);

        public Task<List<BorrowRequest>> GetAll();

        public Task<BorrowRequest> GetById(int entityId);

        public Task<BorrowRequest> Update(BorrowRequest borrowRequest);
    }
}
