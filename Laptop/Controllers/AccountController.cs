using Laptop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Laptop.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError(string.Empty, "");
                return View("Login");
            }    
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View("Login"); 
            }
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {

                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Address) || string.IsNullOrEmpty(user.Password))
                {
                    ModelState.AddModelError(string.Empty, "");
                    return View("Register");
                }
                AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email , Address = user.Address};
				IdentityResult result = await _userManager.CreateAsync(newUser,user.Password);
				
                
				if (result.Succeeded)
                {
                    IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(newUser, "User");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                return View("Register");
            }    
        
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
