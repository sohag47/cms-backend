using AutoMapper;
using cms_backend.DTOs.Categories;
using cms_backend.Enums;
using cms_backend.Models;
using cms_backend.Models.Base;
using cms_backend.Repositories;
using FluentValidation;
using Humanizer;
using Microsoft.AspNetCore.Mvc;


namespace cms_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository repo;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository repository, IMapper mapper)
        {
            this.repo = repository;
            this.mapper = mapper;
        }


        [HttpGet]  // GET: api/categories
        public async Task<IActionResult> List([FromQuery] CategoryQueryDto query)
        {
            var result = await repo.GetPagedAsync(query);
            return Ok(result);
        }


        [HttpGet("{id:int}")] // GET: api/categories/5
        public async Task<IActionResult> GetById(int id)
        {
            var item = await repo.GetByIdAsync(id);

            if (item == null)
                return NotFound(ApiResponse<string>.Fail("Category not found."));

            var category = mapper.Map<CategoryResponseDto>(item);
            return Ok(ApiResponse<CategoryResponseDto>.Ok("Category found successfully", category));
        }

        
        
        [HttpPost] // POST: api/categories
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto request)
        {
            if (request.ParentId.HasValue)
            {
                var parentExists = await repo.ExistsAsync(c => c.Id == request.ParentId.Value);
                if (!parentExists)
                    return BadRequest(ApiResponse<string>.Fail("Parent category not found."));
            }

            var category = new Category
            {
                Name = request.Name,
                Slug = request.Slug,
                ParentId = request.ParentId,
                Status = request.Status,
            };

            await repo.AddAsync(category);
            await repo.SaveChangesAsync();

            var responseDto = mapper.Map<CategoryResponseDto>(category);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, ApiResponse<CategoryResponseDto>.Ok("Category created successfully.", responseDto));
        }




        [HttpPut("{id:int}")] // PUT: api/categories/5
        public async Task<ActionResult<Category>> Update(int id, [FromBody] Category dto)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity is null)
            {
                return NotFound(ApiResponse<string>.Fail("Category not found."));
            }

            if (dto.ParentId.HasValue)
            {
                if (dto.ParentId.Value == id)
                    return BadRequest(ApiResponse<string>.Fail("Category cannot be its own parent."));

                var parentExists = await repo.ExistsAsync(c => c.Id == dto.ParentId.Value);
                if (!parentExists)
                    return BadRequest(ApiResponse<string>.Fail("Parent category not found."));
            }

            entity.Name = dto.Name;
            entity.Slug = dto.Slug;
            entity.ParentId = dto.ParentId;
            entity.Status = dto.Status;

            repo.Update(entity);
            await repo.SaveChangesAsync();

            var responseDto = mapper.Map<CategoryResponseDto>(entity);
            return Ok(ApiResponse<CategoryResponseDto>.Ok("Category updated successfully.", responseDto));
        }
        


        [HttpDelete("{id:int}")] // DELETE: api/categories/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await repo.GetByIdAsync(id);
            if (category == null) return NotFound(ApiResponse<string>.Fail("Category not found."));

            repo.Delete(category);
            await repo.SaveChangesAsync();

            return Ok(ApiResponse<string>.Ok("Category deleted successfully."));
        }



        
        [HttpGet("dropdown")] // GET: api/categories/dropdown
        public async Task<IActionResult> GetDropdown([FromQuery] CategoryDropdownQueryDto query)
        {
            var result = await repo.GetDropdownAsync(query);
            return Ok(result);
        }


        
        [HttpPost("import-csv")] // GET: api/categories/import-csv
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
                    continue; // Skip header

                var parts = line.Split(',');

                if (parts.Length < 4)
                    continue; // Skip invalid rows

                var name = parts[0].Trim();
                var parentIdText = parts[1].Trim();
                var statusText = parts[2].Trim();
                var slug = parts[3].Trim();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(slug))
                    continue;

                int? parentId = int.TryParse(parentIdText, out var pid) ? pid : null;
                bool statusParsed = Enum.TryParse<CategoryStatus>(statusText, true, out var status);

                if (!statusParsed)
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

            if (categories.Count == 0)
                return BadRequest(ApiResponse<string>.Fail("No valid category data found."));

            // Sort so parents (ParentId == null) come first
            var orderedCategories = categories.OrderBy(c => c.ParentId.HasValue ? 1 : 0).ToList();

            // Use your repository methods
            await repo.AddRangeAsync(orderedCategories);
            await repo.SaveChangesAsync();

            return Ok(ApiResponse<string>.Ok($"Successfully imported {orderedCategories.Count} categories."));
        }

    }
}
