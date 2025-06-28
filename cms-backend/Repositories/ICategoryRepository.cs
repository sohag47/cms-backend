using cms_backend.DTOs;
using cms_backend.DTOs.Categories;
using cms_backend.Models;
using cms_backend.Models.Base;
using System.Linq.Expressions;

namespace cms_backend.Repositories
{
    public interface ICategoryRepository : IRepository<Category> 
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<ApiResponse<PagedResponseDto<CategoryResponseDto>>> GetPagedAsync(CategoryQueryDto query);
        Task<ApiResponse<IEnumerable<DropdownResponseDto>>> GetDropdownAsync(CategoryDropdownQueryDto query);
    }
}
