using API.DTOs.Categories;
using Core.Models;
using Core.Models.Base;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IRepository<Category> categoryRepository) : ControllerBase
{
    private readonly IRepository<Category> _categoryRepository = categoryRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.Query()
            .Include(x => x.Parent)
            .OrderBy(x => x.Name)
            .Select(x => new CategoryListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                ParentId = x.ParentId,
                ParentName = x.Parent != null ? x.Parent.Name : null,
                Status = x.Status
            })
            .ToListAsync();

        return Ok(ApiResponse<List<CategoryListItemDto>>.Ok("Categories fetched successfully.", categories));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryRepository.Query()
            .Include(x => x.Parent)
            .Where(x => x.Id == id)
            .Select(x => new CategoryListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                ParentId = x.ParentId,
                ParentName = x.Parent != null ? x.Parent.Name : null,
                Status = x.Status
            })
            .FirstOrDefaultAsync();

        if (category is null)
        {
            return NotFound(ApiResponse<string>.Fail("Category not found."));
        }

        return Ok(ApiResponse<CategoryListItemDto>.Ok("Category fetched successfully.", category));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        if (await _categoryRepository.ExistsAsync(x => x.Slug == dto.Slug))
        {
            return Conflict(ApiResponse<string>.Fail("Slug already exists."));
        }

        var category = new Category
        {
            Name = dto.Name.Trim(),
            Slug = dto.Slug.Trim().ToLowerInvariant(),
            ParentId = dto.ParentId,
            Status = dto.Status
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            ApiResponse<CategoryListItemDto>.Ok("Category created successfully.", new CategoryListItemDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                ParentId = category.ParentId,
                Status = category.Status
            }));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound(ApiResponse<string>.Fail("Category not found."));
        }

        var duplicateSlugExists = await _categoryRepository.Query()
            .AnyAsync(x => x.Slug == dto.Slug && x.Id != id);
        if (duplicateSlugExists)
        {
            return Conflict(ApiResponse<string>.Fail("Slug already exists."));
        }

        category.Name = dto.Name.Trim();
        category.Slug = dto.Slug.Trim().ToLowerInvariant();
        category.ParentId = dto.ParentId;
        category.Status = dto.Status;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        return Ok(ApiResponse<string>.Ok("Category updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound(ApiResponse<string>.Fail("Category not found."));
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();

        return Ok(ApiResponse<string>.Ok("Category deleted successfully."));
    }
}
