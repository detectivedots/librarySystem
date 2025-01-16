using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<List<Category>> GetAll();
    }
}
