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
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = "")
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "")
        {
            try
            {
                var external = await _signInManager.GetExternalLoginInfoAsync();

                var user = new UserFromExternalDto()
                {
                    Email = external.Principal.FindFirstValue(ClaimTypes.Email),
                    Provider = external.ProviderDisplayName,
                    Name = external.Principal.Identity?.Name ?? ""
                };

                var userExists = await _userManager.FindByEmailAsync(user.Email);

                if (userExists != null)
                {
                    await _signInManager.SignInAsync(userExists, true);
                }
                else
                {
                    var identityUser = new AppUserModel()
                    {
                        Email = user.Email,
                        UserName = user.Email.Split("@")[0],
                        NormalizedEmail = user.Email.ToUpper(),
                        NormalizedUserName = user.Email.Split("@")[0].ToUpper(),
                        Address = ""
                    };

                    const string defaultPassword = "CodeMega@123";

                    await _userManager.CreateAsync(identityUser, defaultPassword);

                    IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(identityUser, "User");

                    var newUser = await _userManager.FindByEmailAsync(user.Email);

                    await _signInManager.SignInAsync(newUser, true);
                }

                // TO DO HERE
                // Code tiếp phần lưu các thông tin khác người dùng ở đây

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return new RedirectResult($"~/#error-oauth2");
            }
        }
            public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
