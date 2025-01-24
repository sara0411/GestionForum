using Microsoft.AspNetCore.Mvc;
using ForumRecrutementApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ForumRecrutementApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

[Area("Candidate")]
[AllowAnonymous]
public class CandidatsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CandidatsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        Console.WriteLine("Index: Fetching list of candidats");
        var candidats = await _context.Candidats.ToListAsync();
        Console.WriteLine($"Index: Found {candidats.Count} candidats");
        return View(candidats);
    }

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
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await candidat.CVFile.CopyToAsync(memoryStream);
                    candidat.CVData = memoryStream.ToArray();
                    candidat.CVFileName = candidat.CVFile.FileName;
                    candidat.CVContentType = candidat.CVFile.ContentType;
                }
                Console.WriteLine($"Create POST: CV file stored in memory: {candidat.CVFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create POST: Error processing CV file: {ex.Message}");
                ModelState.AddModelError("CVFile", "An error occurred while processing the CV file.");
            }
        }
        else
        {
            Console.WriteLine("Create POST: No CV file uploaded");
            ModelState.AddModelError("CVFile", "Please upload a CV file.");
        }

        if (candidat.ForumId.HasValue && candidat.ForumId > 0)
        {
            var forum = await _context.Forums.FindAsync(candidat.ForumId);
            if (forum == null)
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
            Console.WriteLine("Edit POST: New CV file uploaded");
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await candidat.CVFile.CopyToAsync(memoryStream);
                    candidat.CVData = memoryStream.ToArray();
                    candidat.CVFileName = candidat.CVFile.FileName;
                    candidat.CVContentType = candidat.CVFile.ContentType;
                }
                Console.WriteLine($"Edit POST: New CV file processed: {candidat.CVFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Edit POST: Error processing CV file: {ex.Message}");
                ModelState.AddModelError("CVFile", "An error occurred while processing the CV file.");
            }
        }
        else
        {
            Console.WriteLine("Edit POST: No new CV file uploaded, keeping existing CV");
            candidat.CVData = existingCandidat.CVData;
            candidat.CVFileName = existingCandidat.CVFileName;
            candidat.CVContentType = existingCandidat.CVContentType;
        }

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Edit POST: ModelState is invalid");
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
        catch (Exception ex)
        {
            Console.WriteLine($"Edit POST: Error updating candidat: {ex.Message}");
            ModelState.AddModelError("", "An error occurred while saving the changes.");
            ViewBag.Forums = new SelectList(await _context.Forums.ToListAsync(), "Id", "Nom", candidat.ForumId);
            return View(candidat);
        }
    }

    // Add this new action to download CV files
    public async Task<IActionResult> DownloadCV(int id)
    {
        var candidat = await _context.Candidats.FindAsync(id);
        if (candidat == null || candidat.CVData == null)
        {
            return NotFound();
        }

        return File(candidat.CVData, candidat.CVContentType, candidat.CVFileName);
    }
}