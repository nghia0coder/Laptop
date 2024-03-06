using Laptop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GiayDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminController : Controller
    {
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		public AdminController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[Authorize(Roles ="Manager, Staff")]
        public IActionResult Index()
        {
            return View();
        }
		[Route("admin/login")]
        public async Task<IActionResult> Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
		{
			if (user.UserName != "admin@gmail.com" && user.UserName != "staff@gmail.com")
			{
				return View(user);
			}	
			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Admin");
			}
			ModelState.AddModelError("", "Error");
			return RedirectToAction("Index", "Admin");
		}
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
			return Redirect("/Home/Index");
        }

    }
}
