using cms_backend.DTOs.Categories;
using cms_backend.Models;
using System.Linq.Expressions;

namespace cms_backend.Repositories
{
    public interface ICategoryRepository : IRepository<Category> 
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto dto);
    }
}
