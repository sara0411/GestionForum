 using ForumRecrutementApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var dashboard = new
            {
                TotalCandidats = _context.Candidats.Count(),
                TotalForums = _context.Forums.Count()
            };
            return View(dashboard);
        }
    }