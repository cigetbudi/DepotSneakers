using DepotSneakers.Models;
using DepotSneakers.Models.ViewModels;
using DepotSneakers.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepotSneakers.Controllers
{
    public class AkunController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AkunController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {            
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "gagal login");
            }
            return PartialView(model);
        }




        public async Task <IActionResult> Daftar()
        {
            if (!_roleManager.RoleExistsAsync(Helper.Owner).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Owner));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
            }
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Daftar(DaftarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> Keluar()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Akun");
        }
    }
}

