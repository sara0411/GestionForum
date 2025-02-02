using ForumRecrutementApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdministrateursController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdministrateursController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        ViewBag.ForumsCount = _context.Forums.Count();
        ViewBag.CandidatsCount = _context.Candidats.Count();
        ViewBag.RecruteursCount = _context.Recruteurs.Count();
        return View();
    }

    [HttpGet]

    public IActionResult Candidats()
    {
        var candidats = _context.Candidats.ToList();
        return View(candidats);
    }

    [HttpGet]

    public IActionResult Recruteurs()
    {
        var recruteurs = _context.Recruteurs.ToList(); // Fetch recruiters
        return View(recruteurs); // Pass recruiters to the view
    }
}