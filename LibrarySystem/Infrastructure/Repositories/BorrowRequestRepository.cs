using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories
{
    public class BorrowRequestRepository : GenericRepository<BorrowRequest>, IBorrowRequestRepository
    {
        private readonly DbSet<BorrowRequest> _borrowRequests;
        private readonly ApplicationDbContext _context;
        public BorrowRequestRepository(ApplicationDbContext context) : base(context)
        {
            _borrowRequests = context.Set<BorrowRequest>();
            _context = context;
        }

        public async Task<List<BorrowRequest>> GetAll()
        {
            var BorrowRequests = await _context.BorrowRequests.ToListAsync();
            return BorrowRequests;
        }

    }
}
