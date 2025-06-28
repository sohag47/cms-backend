using AutoMapper;
using cms_backend.Data;
using cms_backend.DTOs;
using cms_backend.DTOs.Categories;
using cms_backend.DTOs.Posts;
using cms_backend.Enums;
using cms_backend.Models;
using cms_backend.Models.Base;
using cms_backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace cms_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryRepository _Repo, IMapper _mapper, ApplicationDbContext _context) : ControllerBase
    {
        private readonly ICategoryRepository repo = _Repo;
        private readonly IMapper mapper = _mapper;
        protected readonly ApplicationDbContext context = _context;



        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CategoryQueryDto query)
        {
            var result = await repo.GetPagedAsync(query);
            return Ok(result);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var category = await repo.GetByIdAsync(id);

            if (category == null)
                return NotFound(ApiResponse<string>.Fail("Category not found."));

            var categoryDtos = mapper.Map<CategoryResponseDto>(category);
            return Ok(ApiResponse<CategoryResponseDto>.Ok(categoryDtos, "Category found successfully"));
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category updatedPost)
        {
            var category = await repo.GetByIdAsync(id);
            if (category == null)
                return NotFound(ApiResponse<string>.Fail("Post not found."));

            category.Name = updatedPost.Name;
            category.Slug = updatedPost.Slug;
            category.Parent = updatedPost.Parent;
            category.Status = updatedPost.Status;

            repo.Update(category);
            await repo.SaveChangesAsync();

            return CreatedAtAction(nameof(Show), new { id = category.Id },
                ApiResponse<Category>.Ok(category, "Category updated successfully."));
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] CategoryCreateDto dto)
        {
            var responseDto = await repo.CreateCategoryAsync(dto);

            return CreatedAtAction(nameof(Show), new { id = responseDto.Id },
                ApiResponse<CategoryResponseDto>.Ok(responseDto, "Category created successfully."));
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await repo.GetByIdAsync(id);
            if (category == null) return NotFound(ApiResponse<string>.Fail("Category not found."));

            repo.Delete(category);
            await repo.SaveChangesAsync();
            return Ok(ApiResponse<string>.Ok(null, "Category deleted successfully."));
        }

        // GET: api/Categories/dropdown
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown([FromQuery] CategoryDropdownQueryDto query)
        {
            var result = await repo.GetDropdownAsync(query);
            return Ok(result);
        }

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
