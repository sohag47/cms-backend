using Admin.Models.Categories;
using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Admin.Controllers;

public class CategoriesController(IRepository<Category> categoryRepository) : Controller
{
    private readonly IRepository<Category> _categoryRepository = categoryRepository;

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepository.Query()
            .Include(x => x.Parent)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return View(categories);
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryRepository.Query()
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View(new CategoryFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormViewModel model)
    {
        if (await _categoryRepository.ExistsAsync(x => x.Slug == model.Slug.Trim().ToLowerInvariant()))
        {
            ModelState.AddModelError(nameof(model.Slug), "Slug already exists.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model.ParentId);
            return View(model);
        }

        var category = new Category
        {
            Name = model.Name.Trim(),
            Slug = model.Slug.Trim().ToLowerInvariant(),
            ParentId = model.ParentId,
            Status = model.Status
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        var model = new CategoryFormViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            ParentId = category.ParentId,
            Status = category.Status
        };

        await PopulateDropdownsAsync(model.ParentId, category.Id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        var slug = model.Slug.Trim().ToLowerInvariant();
        var duplicateSlugExists = await _categoryRepository.Query()
            .AnyAsync(x => x.Slug == slug && x.Id != id);
        if (duplicateSlugExists)
        {
            ModelState.AddModelError(nameof(model.Slug), "Slug already exists.");
        }

        if (model.ParentId == id)
        {
            ModelState.AddModelError(nameof(model.ParentId), "A category cannot be its own parent.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model.ParentId, id);
            return View(model);
        }

        category.Name = model.Name.Trim();
        category.Slug = slug;
        category.ParentId = model.ParentId;
        category.Status = model.Status;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepository.Query()
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownsAsync(int? selectedParentId = null, int? excludeId = null)
    {
        var parentQuery = _categoryRepository.Query();

        if (excludeId.HasValue)
        {
            parentQuery = parentQuery.Where(x => x.Id != excludeId.Value);
        }

        var parentCategories = await parentQuery
            .OrderBy(x => x.Name)
            .ToListAsync();

        ViewBag.ParentCategories = new SelectList(parentCategories, nameof(Category.Id), nameof(Category.Name), selectedParentId);
    }
}
