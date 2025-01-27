using ForumRecrutementApp.Data;
using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ForumRecrutementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            try
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("Dashboard", "Administrateurs");

                    if (await _userManager.IsInRoleAsync(user, "Recruteur"))
                        return RedirectToAction("Index", "Recruteurs", new { area = "Recruiter" });

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                ModelState.AddModelError("", "An error occurred. Please try again later.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterRecruiter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRecruiter(RegisterRecruiterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Recruteur");

                    var recruteur = new Recruteur
                    {
                        Nom = model.Nom,
                        Entreprise = model.Entreprise,
                        Email = model.Email,
                        IdentityUserId = user.Id
                    };

                    _context.Recruteurs.Add(recruteur);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"New recruiter registered: {model.Email}");

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Recruteurs");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during recruiter registration.");
                ModelState.AddModelError("", "An error occurred. Please try again later.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");

                    var admin = new Administrateur
                    {
                        Email = model.Email,
                        IdentityUserId = user.Id
                    };

                    _context.Administrateurs.Add(admin);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"New admin registered: {model.Email}");

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Dashboard", "Administrateurs");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during admin registration.");
                ModelState.AddModelError("", "An error occurred. Please try again later.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout.");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
