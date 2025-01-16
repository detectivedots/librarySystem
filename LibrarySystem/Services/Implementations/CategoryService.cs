using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services.Implementations
{
    public class CategoryService: ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Category> Add(Category entity)
        {
            if (entity is not null)
                await _unitOfWork.CategoryRepository.AddAsync(entity);
            return entity;
        }

        public async Task<int> Delete(int entityId)
        {
            var category = await GetById(entityId);
            if (category == null)
                return -1; // Not found..!
            await _unitOfWork.CategoryRepository.DeleteAsync(category);
            return 1;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _unitOfWork.CategoryRepository.GetAll();
        }

        public async Task<Category> GetById(int entityId)
        {
            return await _unitOfWork.CategoryRepository.GetByIdAsync(entityId);
        }

        public async Task<Category> Update(Category category)
        {
            if (category is not null)
                await _unitOfWork.CategoryRepository.UpdateAsync(category);
            return category;
        }
    }
}
