using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.Repositories;
using LibrarySystem.Models;

namespace LibrarySystem.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<Category> Add(Category entity);

        public Task<int> Delete(int entityId);

        public Task<List<Category>> GetAll();

        public Task<Category> GetById(int entityId);

        public Task<Category> Update(Category category);
    }
}
