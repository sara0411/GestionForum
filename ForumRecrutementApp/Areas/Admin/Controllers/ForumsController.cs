﻿using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Mvc;
using ForumRecrutementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ForumRecrutementApp.Areas.Admin.Controllers
{
   [Area("Admin")]
public class ForumsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ForumsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Forums.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Forum forum)
    {
        if (ModelState.IsValid)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(forum);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var forum = await _context.Forums.FindAsync(id);
        if (forum == null)
            return NotFound();

        return View(forum);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Forum forum)
    {
        if (id != forum.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(forum.Id))
                    return NotFound();
                throw;
            }
        }
        return View(forum);
    }

    private bool ForumExists(int id)
    {
        return _context.Forums.Any(e => e.Id == id);
    }
}
}
