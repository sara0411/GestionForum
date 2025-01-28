using ForumRecrutementApp.Data;
using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ForumRecrutementApp.Areas.Recruiter.Controllers
{
    [Area("Recruiter")]
    [Authorize(Roles = "Recruteur")]
    public class RecruteursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RecruteursController> _logger;

        public RecruteursController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<RecruteursController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found while accessing Index");
                    return RedirectToAction("Login", "Account");
                }

                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation($"User {user.Email} with roles: {string.Join(", ", roles)} accessing Index");

                var candidats = from c in _context.Candidats
                                select c;
                if (!string.IsNullOrEmpty(search))
                {
                    candidats = candidats.Where(s => s.Competences.Contains(search));
                }
                return View(await candidats.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Index action");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Evaluer(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var recruteur = await _context.Recruteurs
                    .FirstOrDefaultAsync(r => r.IdentityUserId == user.Id);

                if (recruteur == null)
                {
                    _logger.LogWarning($"Recruiter not found for user {user.Email}");
                    return RedirectToAction("Error", "Home");
                }

                var candidat = await _context.Candidats.FirstOrDefaultAsync(c => c.Id == id);
                if (candidat == null)
                {
                    _logger.LogWarning($"Candidate not found with ID: {id}");
                    return NotFound();
                }

                ViewBag.CandidatId = id;
                ViewBag.CandidatNom = candidat.Nom;
                ViewBag.RecruteurId = recruteur.Id;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Evaluer GET action");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Evaluer(int id, [Bind("Note,Commentaire")] Evaluation evaluation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var recruteur = await _context.Recruteurs
                        .FirstOrDefaultAsync(r => r.IdentityUserId == user.Id);

                    if (recruteur == null)
                    {
                        _logger.LogWarning($"Recruiter not found for user {user.Email}");
                        ModelState.AddModelError("", "Recruteur non trouvé.");
                        return View(evaluation);
                    }

                    evaluation.CandidatId = id;
                    evaluation.RecruteurId = recruteur.Id;
                    evaluation.DateEvaluation = DateTime.Now;

                    _context.Evaluations.Add(evaluation);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Evaluation created for candidate {id} by recruiter {recruteur.Id}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Evaluer POST action");
                ModelState.AddModelError("", "Une erreur s'est produite lors de l'enregistrement de l'évaluation.");
            }

            ViewBag.CandidatId = id;
            return View(evaluation);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var recruteur = await _context.Recruteurs
                    .FirstOrDefaultAsync(r => r.IdentityUserId == user.Id);

                if (recruteur != null)
                {
                    var evaluationStats = await _context.Evaluations
                        .Where(e => e.RecruteurId == recruteur.Id)
                        .GroupBy(e => e.CandidatId)
                        .Select(g => new
                        {
                            CandidatCount = g.Count(),
                            AverageNote = g.Average(e => e.Note)
                        })
                        .FirstOrDefaultAsync();

                    ViewBag.TotalEvaluations = evaluationStats?.CandidatCount ?? 0;
                    ViewBag.AverageNote = evaluationStats?.AverageNote ?? 0;
                    _logger.LogInformation($"Dashboard loaded for recruiter {recruteur.Id}");
                }
                else
                {
                    _logger.LogWarning($"No recruiter found for user {user.Email}");
                    ViewBag.TotalEvaluations = 0;
                    ViewBag.AverageNote = 0;
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Dashboard action");
                return RedirectToAction("Error", "Home");
            }
        }

    }
}