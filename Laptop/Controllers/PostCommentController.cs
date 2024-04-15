using Laptop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Laptop.Controllers
{
    public class PostCommentController : Controller
    {
        private readonly LaptopContext _context;
        private readonly UserManager<AppUserModel> _userManager;

        public PostCommentController(LaptopContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Create(PostComment postComment, string currentUrl)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            postComment.CustomerId = userId;
            postComment.CreatedDate = DateTime.Now;
            _context.PostComments.Add(postComment);
            await _context.SaveChangesAsync();
            return Redirect(currentUrl);

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
