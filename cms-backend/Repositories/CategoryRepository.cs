using AutoMapper;
using cms_backend.Data;
using cms_backend.DTOs;
using cms_backend.DTOs.Categories;
using cms_backend.Models;
using cms_backend.Models.Base;
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
        public async Task<ApiResponse<PagedResponseDto<CategoryResponseDto>>> GetPagedAsync(CategoryQueryDto query)
        {
            var queryable = context.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                queryable = queryable.Where(c => c.Name.Contains(query.Search));
            }

            if (query.ParentId.HasValue)
            {
                queryable = queryable.Where(c => c.ParentId == query.ParentId.Value);
            }

            if (query.Status.HasValue)
            {
                queryable = queryable.Where(c => c.Status == query.Status.Value);
            }


            var totalRecords = await queryable.CountAsync();
            var items = await queryable
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var categoryDtos = mapper.Map<List<CategoryResponseDto>>(items);

            var pagedResponse = PagedResponseDto<CategoryResponseDto>.Create(
                categoryDtos, query.Page, query.PageSize, totalRecords
            );

            return ApiResponse<PagedResponseDto<CategoryResponseDto>>.Ok(pagedResponse, "Categories fetched successfully");
        }

        public async Task<ApiResponse<IEnumerable<DropdownResponseDto>>> GetDropdownAsync(CategoryDropdownQueryDto query)
        {
            var queryable = context.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                queryable = queryable.Where(c => c.Name.Contains(query.Search));
            }

            if (query.ParentId.HasValue)
            {
                queryable = queryable.Where(c => c.ParentId == query.ParentId.Value);
            }

            if (query.Status.HasValue)
            {
                queryable = queryable.Where(c => c.Status == query.Status.Value);
            }

            var dropdownItems = await queryable
                .Select(c => new DropdownResponseDto
                {
                    Value = c.Id,
                    Label = c.Name
                }).ToListAsync();

            return ApiResponse<IEnumerable<DropdownResponseDto>>.Ok(dropdownItems, "Categories fetched successfully");
        }

    }
}
