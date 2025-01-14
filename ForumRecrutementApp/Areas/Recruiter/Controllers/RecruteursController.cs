using ForumRecrutementApp.Data;
using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Area("Recruiter")]
[Authorize(Roles = "Recruteur")]
public class RecruteursController : Controller
{
    private readonly ApplicationDbContext _context;

    public RecruteursController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string search)
    {
        var candidats = from c in _context.Candidats select c;

        if (!string.IsNullOrEmpty(search))
        {
            candidats = candidats.Where(s => s.Competences.Contains(search));
        }

        return View(await candidats.ToListAsync());
    }

    [HttpGet]
    public IActionResult Evaluer(int id)
    {
        ViewBag.CandidatId = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Evaluer(int id, Evaluation evaluation)
    {
        evaluation.CandidatId = id;
        _context.Evaluations.Add(evaluation);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
