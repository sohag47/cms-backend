using AutoMapper;
using cms_backend.Data;
using cms_backend.DTOs.Categories;
using cms_backend.DTOs.Posts;
using cms_backend.Models;
using cms_backend.Models.Base;
using cms_backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cms_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryRepository _Repo, IMapper _mapper) : ControllerBase
    {
        private readonly ICategoryRepository repo = _Repo;
        private readonly IMapper mapper = _mapper;

       

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CategoryQueryDto query)
        {
            //var categories = await repo.GetAllAsync();
            //var categoryDtos = mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            //return Ok(ApiResponse<IEnumerable<CategoryResponseDto>>.Ok(categoryDtos));
            return Ok(query);
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


        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown([FromQuery] CategoryQueryDto query)
        {
            //var categories = await repo.GetAllAsync();
            //var categoryDtos = mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            //return Ok(ApiResponse<IEnumerable<CategoryResponseDto>>.Ok(categoryDtos));
            return  Ok(query);
        }

    }
}
