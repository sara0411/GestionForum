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
        public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
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

                // Base query
                var query = _context.Candidats.AsQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.Competences.Contains(search));
                }

                // Pagination
                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var candidats = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include(c => c.Forum) // Include related data if needed
                    .ToListAsync();

                // Pass data to the view
                ViewBag.PageIndex = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.HasNextPage = page < totalPages;
                ViewBag.Search = search; // Pass the search term back to the view

                return View(candidats);
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

                // Pass data to the view
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
            var user1 = await _userManager.GetUserAsync(User);
            var recruteur = await _context.Recruteurs
                .FirstOrDefaultAsync(r => r.IdentityUserId == user1.Id);

            if (recruteur == null)
            {
                // Create a new Recruteur record
                recruteur = new Recruteur
                {
                    IdentityUserId = user1.Id,
                    Nom = user1.UserName, // Or any other required fields
                    Email = user1.Email
                };

                _context.Recruteurs.Add(recruteur);
                await _context.SaveChangesAsync();
            }

            evaluation.RecruteurId = recruteur.Id;
            _logger.LogInformation("Evaluer POST action called."); // Add this line
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("ModelState is valid."); // Add this line
                    var user = await _userManager.GetUserAsync(User);
                    _logger.LogInformation($"Current user ID: {user.Id}"); // Add this line

                    //var recruteur = await _context.Recruteurs
                       // .FirstOrDefaultAsync(r => r.IdentityUserId == user.Id);

                    if (recruteur == null)
                    {
                        _logger.LogWarning($"Recruiter not found for user {user.Email}");
                        ModelState.AddModelError("", "Recruteur non trouvé.");
                        return View(evaluation);
                    }

                    _logger.LogInformation($"Recruteur ID: {recruteur.Id}"); // Add this line
                    evaluation.RecruteurId = recruteur.Id;

                    // Set evaluation properties
                    evaluation.CandidatId = id;
                    evaluation.DateEvaluation = DateTime.Now;

                    // Add the evaluation to the database
                    _context.Evaluations.Add(evaluation);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Evaluation created for candidate {id} by recruiter {recruteur.Id}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogWarning("ModelState is invalid: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))); // Add this line
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Evaluer POST action");
                ModelState.AddModelError("", "Une erreur s'est produite lors de l'enregistrement de l'évaluation.");
            }



            // If we reach here, something went wrong
            ViewBag.CandidatId = id;
            return View(evaluation);
        }

        /*  [HttpGet]
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
          }*/

        [HttpGet]
        public async Task<IActionResult> Statistiques()
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

                // Fetch statistics for evaluations
                var evaluationStats = await _context.Evaluations
                    .Where(e => e.RecruteurId == recruteur.Id)
                    .GroupBy(e => e.CandidatId)
                    .Select(g => new
                    {
                        CandidatCount = g.Count(),
                        AverageNote = g.Average(e => e.Note)
                    })
                    .FirstOrDefaultAsync();

                // Pass data to the view
                ViewBag.TotalEvaluations = evaluationStats?.CandidatCount ?? 0;
                ViewBag.AverageNote = evaluationStats?.AverageNote ?? 0;

                _logger.LogInformation($"Statistics loaded for recruiter {recruteur.Id}");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Statistiques action");
                return RedirectToAction("Error", "Home");
            }
        }



    }
}