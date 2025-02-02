using Microsoft.AspNetCore.Mvc;
using ForumRecrutementApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ForumRecrutementApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

[Area("Candidate")]
[AllowAnonymous]

public class CandidatsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CandidatsController> _logger;


    public CandidatsController(
      ApplicationDbContext context,
      ILogger<CandidatsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Candidats/Index
    public async Task<IActionResult> Index()
    {
        try
        {
            var candidates = await _context.Candidats
                .Include(c => c.Forum)
                .ToListAsync();

            return View(candidates);
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError(ex, "Error fetching candidates");
            return View("Error");
        }
    }

// GET: Candidats/Create
public IActionResult Create()
    {
        Console.WriteLine("Create GET: Preparing form");
        var forums = _context.Forums.ToList();
        if (forums == null || !forums.Any())
        {
            Console.WriteLine("Create GET: No forums found");
            throw new Exception("No forums found. Please ensure the Forums table is populated.");
        }

        ViewBag.Forums = new SelectList(forums, "Id", "Nom");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Candidat candidat)
    {
        Console.WriteLine("Create POST: Form submitted");
        Console.WriteLine($"Create POST: Received ForumId value: {candidat.ForumId}");

        ModelState.Remove("CV");
        ModelState.Remove("Forum");
        ModelState.Remove("ForumId");

        if (candidat.CVFile != null && candidat.CVFile.Length > 0)
        {
            Console.WriteLine("Create POST: CV file uploaded");
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Path.GetFileNameWithoutExtension(candidat.CVFile.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(candidat.CVFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await candidat.CVFile.CopyToAsync(stream);
                }

                candidat.CV = fileName;
                Console.WriteLine($"Create POST: CV file saved as {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create POST: Error saving CV file: {ex.Message}");
                ModelState.AddModelError("CVFile", "An error occurred while saving the CV file.");
            }
        }
        else
        {
            Console.WriteLine("Create POST: No CV file uploaded");
            ModelState.AddModelError("CVFile", "Please upload a CV file.");
        }

        // Validate Forum selection
        if (candidat.ForumId.HasValue && candidat.ForumId > 0)
        {
            var forum = await _context.Forums.FindAsync(candidat.ForumId);
            if (forum != null)
            {
                Console.WriteLine($"Create POST: Found valid forum: {forum.Nom}");
                // Don't set candidat.Forum here as it may cause tracking issues
            }
            else
            {
                Console.WriteLine($"Create POST: Forum with ID {candidat.ForumId} not found in database");
                ModelState.AddModelError("ForumId", "Selected Forum does not exist.");
            }
        }
        else
        {
            Console.WriteLine("Create POST: No valid ForumId provided");
            ModelState.AddModelError("ForumId", "Please select a Forum.");
        }

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Create POST: ModelState is invalid");
            foreach (var error in ModelState)
            {
                Console.WriteLine($"Key: {error.Key}");
                foreach (var err in error.Value.Errors)
                {
                    Console.WriteLine($"  Error: {err.ErrorMessage}");
                }
            }

            ViewBag.Forums = new SelectList(await _context.Forums.ToListAsync(), "Id", "Nom");
            return View(candidat);
        }

        try
        {
            // Only add the Candidat without trying to attach the Forum
            _context.Candidats.Add(candidat);
            await _context.SaveChangesAsync();
            Console.WriteLine("Create POST: Candidat saved successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Create POST: Error saving to database: {ex.Message}");
            ModelState.AddModelError("", "An error occurred while saving the candidate.");
            ViewBag.Forums = new SelectList(await _context.Forums.ToListAsync(), "Id", "Nom");
            return View(candidat);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        Console.WriteLine($"Edit GET: Called with id = {id}");
        if (id == null)
        {
            Console.WriteLine("Edit GET: id is null");
            return NotFound();
        }

        var candidat = await _context.Candidats
            .Include(c => c.Forum)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (candidat == null)
        {
            Console.WriteLine($"Edit GET: No candidat found with id = {id}");
            return NotFound();
        }

        ViewBag.Forums = new SelectList(_context.Forums, "Id", "Nom", candidat.ForumId);
        Console.WriteLine("Edit GET: Form prepared with forums dropdown");
        return View(candidat);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Candidat candidat)
    {
        Console.WriteLine($"Edit POST: Called with id = {id}");

        if (id != candidat.Id)
        {
            Console.WriteLine("Edit POST: Mismatch between route id and candidat id");
            return NotFound();
        }

        ModelState.Remove("CV");
        ModelState.Remove("Forum");
        ModelState.Remove("ForumId");

        var existingCandidat = await _context.Candidats.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingCandidat == null)
        {
            Console.WriteLine($"Edit POST: No existing candidat found with id = {id}");
            return NotFound();
        }

        if (candidat.CVFile != null && candidat.CVFile.Length > 0)
        {
            Console.WriteLine("Edit POST: CV file uploaded");
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Path.GetFileNameWithoutExtension(candidat.CVFile.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(candidat.CVFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await candidat.CVFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(existingCandidat.CV))
                {
                    var oldFilePath = Path.Combine(uploadsFolder, existingCandidat.CV);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                candidat.CV = fileName;
                Console.WriteLine($"Edit POST: New CV file saved as {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Edit POST: Error saving CV file: {ex.Message}");
                ModelState.AddModelError("CVFile", "An error occurred while saving the CV file.");
            }
        }
        else
        {
            Console.WriteLine("Edit POST: No new CV file uploaded, keeping existing CV");
            candidat.CV = existingCandidat.CV; 
        }

        if (candidat.ForumId.HasValue && candidat.ForumId > 0)
        {
            var forum = await _context.Forums.FindAsync(candidat.ForumId);
            if (forum == null)
            {
                Console.WriteLine($"Edit POST: Forum with ID {candidat.ForumId} not found in database");
                ModelState.AddModelError("ForumId", "Selected Forum does not exist.");
            }
        }
        else
        {
            Console.WriteLine("Edit POST: No valid ForumId provided");
            ModelState.AddModelError("ForumId", "Please select a Forum.");
        }

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Edit POST: ModelState is invalid");
            foreach (var error in ModelState)
            {
                Console.WriteLine($"Key: {error.Key}");
                foreach (var err in error.Value.Errors)
                {
                    Console.WriteLine($"  Error: {err.ErrorMessage}");
                }
            }

            ViewBag.Forums = new SelectList(await _context.Forums.ToListAsync(), "Id", "Nom", candidat.ForumId);
            return View(candidat);
        }

        try
        {
            _context.Entry(candidat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            Console.WriteLine("Edit POST: Candidat updated successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!CandidatExists(candidat.Id))
            {
                Console.WriteLine("Edit POST: Candidat not found during concurrency check");
                return NotFound();
            }
            Console.WriteLine($"Edit POST: Concurrency exception occurred: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Edit POST: Error updating candidat: {ex.Message}");
            ModelState.AddModelError("", "An error occurred while saving the changes.");
            ViewBag.Forums = new SelectList(await _context.Forums.ToListAsync(), "Id", "Nom", candidat.ForumId);
            return View(candidat);
        }
    }

    private bool CandidatExists(int id)
    {
        return _context.Candidats.Any(e => e.Id == id);
    }

    // GET: Candidats/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        Console.WriteLine($"Delete GET: Called with id = {id}");
        if (id == null)
        {
            Console.WriteLine("Delete GET: id is null");
            return NotFound();
        }

        var candidat = await _context.Candidats.FirstOrDefaultAsync(c => c.Id == id);
        if (candidat == null)
        {
            Console.WriteLine($"Delete GET: No candidat found with id = {id}");
            return NotFound();
        }

        Console.WriteLine("Delete GET: Candidat found");
        return View(candidat);
    }

    // POST: Candidats/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Console.WriteLine($"Delete POST: Called with id = {id}");
        var candidat = await _context.Candidats.FindAsync(id);
        if (candidat != null)
        {
            _context.Candidats.Remove(candidat);
            await _context.SaveChangesAsync();
            Console.WriteLine("Delete POST: Candidat deleted successfully");
        }
        else
        {
            Console.WriteLine("Delete POST: Candidat not found");
        }

        return RedirectToAction(nameof(Index));
    }


}