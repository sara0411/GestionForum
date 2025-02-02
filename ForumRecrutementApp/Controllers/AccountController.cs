﻿using ForumRecrutementApp.Data;
using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumRecrutementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context; 


        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
                    ApplicationDbContext context) 

        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("Dashboard", "Administrateurs", new { area = "Admin" });
                    else if (await _userManager.IsInRoleAsync(user, "Recruteur"))
                        return RedirectToAction("Index", "Recruteurs", new { area = "Recruiter" });

                    // return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Tentative de connexion invalide");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult RegisterRecruiter()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterRecruiter(RegisterRecruiterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Recruteur");

                    var recruteur = new Recruteur
                    {
                        Email = model.Email,
                        IdentityUserId = user.Id  
                    };

                    _context.Recruteurs.Add(recruteur);
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Recruiter");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Form data received:");
                Console.WriteLine($"Email: {model.Email}");
                Console.WriteLine($"Password: {new string('•', model.Password?.Length ?? 0)}");
                Console.WriteLine($"ConfirmPassword: {new string('•', model.ConfirmPassword?.Length ?? 0)}");
                Console.WriteLine($"Nom: {model.Nom}");

                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"Error for {key}: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            var admin = new Administrateur
            {
                Email = model.Email,
                IdentityUserId = user.Id,
                Nom = model.Nom 
            };

            try
            {
                _context.Administrateurs.Add(admin);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "An error occurred while saving the administrator.");
                return View(model);
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Dashboard", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}