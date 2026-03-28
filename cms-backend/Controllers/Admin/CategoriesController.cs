using cms_backend.Data;
using cms_backend.DTOs.Admin;
using cms_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cms_backend.Controllers.Admin;

[Route("admin/categories")]
public class CategoriesController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Categories
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var categories = _context.Categories.Include(c => c.Parent);
        return View(await categories.ToListAsync());
    }


    // GET: Categories/Details/5
    [HttpGet("details/{id?}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.Include(c => c.Parent).FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }



    // GET: Categories/Create
    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(new CreateCategoryDto());
    }



    // POST: Categories/Create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(dto);
        }

        bool slugExists = await _context.Categories.AnyAsync(c => c.Slug == dto.Slug);

        if (slugExists)
        {
            ModelState.AddModelError("Slug", "slug already used");
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(dto);
        }

        var category = new Category
        {
            Name = dto.Name!,
            Slug = dto.Slug!,
            ParentId = dto.ParentId == 0 ? null : dto.ParentId,
            Status = dto.Status
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Category created successfully!";
        return RedirectToAction("Index");
    }



    // GET: Categories/Edit/5
    [HttpGet("edit/{id?}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);

        var dto = new EditCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            ParentId = category.ParentId,
            Status = category.Status
        };

        return View(dto);
    }



    // POST: Categories/Edit/5
    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditCategoryDto dto)
    {
        if (id != dto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                category.Name = dto.Name;
                category.Slug = dto.Slug;
                category.ParentId = dto.ParentId;
                category.Status = dto.Status;

                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(dto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", dto.ParentId);
        return View(dto);
    }



    // GET: Categories/Delete/5
    [HttpGet("delete/{id?}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.Include(c => c.Parent).FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }



    // POST: Categories/Delete/5
    [HttpPost("delete/{id?}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }



    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}
