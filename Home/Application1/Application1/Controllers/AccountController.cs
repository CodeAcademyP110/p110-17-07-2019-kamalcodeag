using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application1.Database;
using Application1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application1.Controllers
{
    public class AccountController : Controller
    {
        private readonly MySample _context;
        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;
        public AccountController(MySample context, 
                                UserManager<MyUser> userManager,
                                SignInManager<MyUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [ActionName("Register")]
        public IActionResult RegisterGet()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public async Task<IActionResult> RegisterPost(RegisterVM registerVM)
        {
            if(ModelState.IsValid)
            {
                MyUser newUser = new MyUser()
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Age = registerVM.Age,
                    UserName = registerVM.Username,
                    Email = registerVM.Email
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, registerVM.Password);

                if(!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(registerVM);
                }

                await _signInManager.SignInAsync(newUser, isPersistent: false);
            }
            else
            {
                return View(registerVM);
            }
            return RedirectToAction("Index", "Home");
            //return View();
        }

        [HttpGet]
        [ActionName("SignIn")]
        public IActionResult SignInGet()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("SignIn")]
        public async Task<IActionResult> SignInPost(SignInVM signInVM)
        {
            if(!ModelState.IsValid)
            {
                return View(signInVM);
            }

            MyUser user = await _userManager.FindByNameAsync(signInVM.Username);

            if(user == null)
            {
                ModelState.AddModelError("", "İstifadəçi adı və ya şifrə düzgün deyil");
                return View(signInVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, signInVM.Password, signInVM.RememberMe, true);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "İstifadəçi adı və ya şifrə düzgün deyil");
                return View(signInVM);
            }

            return RedirectToAction(nameof(UserProfileController.MyProfile), "UserProfile");
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}