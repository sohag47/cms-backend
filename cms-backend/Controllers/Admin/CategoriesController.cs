using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cms_backend.Data;
using cms_backend.Models;

namespace cms_backend.Controllers.Admin;

[Route("admin/categories")]
public class CategoriesController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Categories
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Categories.Include(c => c.Parent);
        return View(await applicationDbContext.ToListAsync());
    }


    // GET: Categories/Details/5
    [HttpGet("details/{id?}")]
    public async Task<IActionResult> Details(int? id)
    {
        Console.WriteLine($"ID: {id}");
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .Include(c => c.Parent)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }



    // GET: Categories/Create
    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id");
        return View();
    }



    // POST: Categories/Create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Slug,ParentId,Status,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,DeletedAt,DeletedBy,Id")] Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentId);
        return View(category);
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
        ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentId);
        return View(category);
    }



    // POST: Categories/Edit/5
    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,Slug,ParentId,Status,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,DeletedAt,DeletedBy,Id")] Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
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
        ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentId);
        return View(category);
    }



    // GET: Categories/Delete/5
    [HttpGet("delete/{id?}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .Include(c => c.Parent)
            .FirstOrDefaultAsync(m => m.Id == id);
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
