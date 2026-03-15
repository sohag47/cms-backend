using AutoMapper;
using cms_backend.DTOs.Categories;
using cms_backend.Enums;
using cms_backend.Models;
using cms_backend.Models.Base;
using cms_backend.Repositories;
using FluentValidation;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace cms_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IRepository<Category> _repo, IMapper _mapper) : ControllerBase
{
    private readonly IRepository<Category> _repo = _repo;
    private readonly IMapper _mapper = _mapper;


    // GET: api/categories
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] CategoryQueryDto query)
    {
        var pagedCategories = await _repo.GetPagedAsync(
            page: query.Page,
            pageSize: query.PageSize,
            filter: c =>
                (string.IsNullOrEmpty(query.Search) || c.Name.Contains(query.Search)) &&
                (!query.ParentId.HasValue || c.ParentId == query.ParentId),
            includes: q => q.Include(c => c.Parent)
        );

        var result = _mapper.Map<IEnumerable<CategoryResponseDto>>(pagedCategories.Data);

        var response = new 
        {
            pagedCategories.Total,
            pagedCategories.Page,
            pagedCategories.PageSize,
            Data = result
        };

        return Ok(ApiResponse<object>.Ok("Categories fetched successfully.", response));
    }



    // GET: api/categories/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _repo.GetByIdAsync(id);

        if (item == null)
            return NotFound(ApiResponse<string>.Fail("Category not found."));

        var category = _mapper.Map<CategoryResponseDto>(item);

        return Ok(ApiResponse<CategoryResponseDto>
            .Ok("Category found successfully.", category));
    }



    // POST: api/categories
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        if (dto.ParentId.HasValue)
        {
            var parentExists = await _repo.ExistsAsync(c => c.Id == dto.ParentId.Value);

            if (!parentExists)
                return BadRequest(ApiResponse<string>.Fail("Parent category not found."));
        }

        var category = _mapper.Map<Category>(dto);

        await _repo.AddAsync(category);
        await _repo.SaveChangesAsync();

        var responseDto = _mapper.Map<CategoryResponseDto>(category);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            ApiResponse<CategoryResponseDto>.Ok("Category created successfully.", responseDto)
        );
    }



    // PUT: api/categories/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);

        if (entity == null)
            return NotFound(ApiResponse<string>.Fail("Category not found."));

        if (dto.ParentId.HasValue)
        {
            if (dto.ParentId.Value == id)
                return BadRequest(ApiResponse<string>.Fail("Category cannot be its own parent."));

            var parentExists = await _repo.ExistsAsync(c => c.Id == dto.ParentId.Value);

            if (!parentExists)
                return BadRequest(ApiResponse<string>.Fail("Parent category not found."));
        }

        _mapper.Map(dto, entity);

        _repo.Update(entity);
        await _repo.SaveChangesAsync();

        var response = _mapper.Map<CategoryResponseDto>(entity);

        return Ok(ApiResponse<CategoryResponseDto>
            .Ok("Category updated successfully.", response));
    }


    // DELETE: api/categories/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _repo.GetByIdAsync(id);

        if (category == null)
            return NotFound(ApiResponse<string>.Fail("Category not found."));

        _repo.Delete(category);
        await _repo.SaveChangesAsync();

        return Ok(ApiResponse<string>.Ok("Category deleted successfully."));
    }


    // POST: api/categories/import-csv
    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse<string>.Fail("No file uploaded."));

        var categories = new List<Category>();

        using var reader = new StreamReader(file.OpenReadStream());

        int lineNumber = 0;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            lineNumber++;

            if (lineNumber == 1)
                continue;

            var parts = line?.Split(',');

            if (parts == null || parts.Length < 4)
                continue;

            var name = parts[0].Trim();
            var parentIdText = parts[1].Trim();
            var statusText = parts[2].Trim();
            var slug = parts[3].Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(slug))
                continue;

            int? parentId = int.TryParse(parentIdText, out var pid) ? pid : null;

            if (!Enum.TryParse<CategoryStatus>(statusText, true, out var status))
                status = CategoryStatus.Active;

            categories.Add(new Category
            {
                Name = name,
                ParentId = parentId,
                Status = status,
                Slug = slug,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!categories.Any())
            return BadRequest(ApiResponse<string>.Fail("No valid category data found."));

        await _repo.AddRangeAsync(categories);
        await _repo.SaveChangesAsync();

        return Ok(ApiResponse<string>
            .Ok($"Successfully imported {categories.Count} categories."));
    }


    //[HttpGet("dropdown")] // GET: api/categories/dropdown
    //public async Task<IActionResult> GetDropdown([FromQuery] CategoryDropdownQueryDto query)
    //{
    //    var result = await repo.GetDropdownAsync(query);
    //    return Ok(result);
    //}
}
