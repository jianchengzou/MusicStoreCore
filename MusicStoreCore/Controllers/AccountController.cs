using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreCore.ViewModel;
using Microsoft.AspNetCore.Identity;
using MusicStoreCore.Models;
using Microsoft.AspNetCore.Http;

namespace MusicStoreCore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private MusicStoreDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, MusicStoreDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = model.UserName                    
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    MigrateShoppingCart(model.UserName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }

            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Store");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)//result doesn't have errors collection
                {
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        MigrateShoppingCart(model.UserName);
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Store");
                    }                    
                }           
            }
            ModelState.AddModelError("", "Could not login!!");
            return View(model);            
        }

        private void MigrateShoppingCart(string userName)
        {
            var cart = ShoppingCart.GetCart(_context, this.HttpContext);
            cart.MigrateCart(userName);
            this.HttpContext.Session.SetString(ShoppingCart.CartSessionKey, userName);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}