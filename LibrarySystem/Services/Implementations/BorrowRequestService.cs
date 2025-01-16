using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services.Implementations
{
    public class BorrowRequestService: IBorrowRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BorrowRequest> Add(BorrowRequest entity)
        {
            if (entity is not null)
                await _unitOfWork.BorrowRequestRepository.AddAsync(entity);
            return entity;
        }

        public async Task<int> Delete(int entityId)
        {
            var borrowRequest = await GetById(entityId);
            if (borrowRequest == null)
                return -1; // Not found..!
            await _unitOfWork.BorrowRequestRepository.DeleteAsync(borrowRequest);
            return 1;
        }

        public async Task<List<BorrowRequest>> GetAll()
        {
            return await _unitOfWork.BorrowRequestRepository.GetAll();
        }

        public async Task<BorrowRequest> GetById(int entityId)
        {
            return await _unitOfWork.BorrowRequestRepository.GetByIdAsync(entityId);
        }

        public async Task<BorrowRequest> Update(BorrowRequest borrowRequest)
        {
            if (borrowRequest is not null)
                await _unitOfWork.BorrowRequestRepository.UpdateAsync(borrowRequest);
            return borrowRequest;
        }
    }
}
