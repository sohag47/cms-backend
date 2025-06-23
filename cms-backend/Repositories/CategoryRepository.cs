using AutoMapper;
using cms_backend.Data;
using cms_backend.DTOs.Categories;
using cms_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace cms_backend.Repositories
{
    public class CategoryRepository(ApplicationDbContext context, IMapper _mapper) : Repository<Category>(context), ICategoryRepository
    {
        private readonly IMapper mapper = _mapper;



        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var category = mapper.Map<Category>(dto);
            await AddAsync(category);
            await SaveChangesAsync();

            return mapper.Map<CategoryResponseDto>(category);
        }

    }
}
