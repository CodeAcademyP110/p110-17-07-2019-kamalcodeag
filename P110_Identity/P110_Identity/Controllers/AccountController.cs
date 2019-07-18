using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P110_Identity.DAL;
using P110_Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using P110_Identity.Models;
using static P110_Identity.Utilities.SD;

namespace P110_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context, 
                                UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signinManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            ViewBag.Countries = _context.Countries;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _context.Countries;
                return View(registerViewModel);
            }

            ApplicationUser newUser = new ApplicationUser
            {
                Firstname = registerViewModel.Firstname,
                Lastname = registerViewModel.Lastname,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username,
                CityId = registerViewModel.CityId
            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if(!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ViewBag.Countries = _context.Countries;
                return View(registerViewModel);
            }

            //add default member role to user
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());

            await _signinManager.SignInAsync(newUser, true);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel LoginViewModel)
        {
            if (!ModelState.IsValid) return View(LoginViewModel);
            ApplicationUser user = await _userManager.FindByEmailAsync(LoginViewModel.Email);

            if(user == null)
            {
                ModelState.AddModelError("", "Email or password is invalid");
                return View(LoginViewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult =
                 await _signinManager.PasswordSignInAsync(user, LoginViewModel.Password, LoginViewModel.RememberMe, true);

            if(!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is invalid");
                return View(LoginViewModel);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task SeedRoles()
        {
            if(!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Manager.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Member.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Member.ToString()));
            }
        }
    }
}