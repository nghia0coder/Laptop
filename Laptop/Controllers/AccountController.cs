using Laptop.ViewModels;
using Laptop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Laptop.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private readonly LaptopContext _context;
        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, LaptopContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
        public async Task<IActionResult> Register(RegisterViewModel user)
        {

            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Address) || string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError(string.Empty, "");
                return View("Register");
            }
            AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email };


            IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Register");
            }

            Customer customer = new Customer
            {
                Name = user.Name,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                AccountId = newUser.Id,
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();


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
