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
        //kkkk
        [HttpPost]
        public async Task<IActionResult> Create(PostComment postComment, string currentUrl)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var customerId = _context.Customers.Where(n => n.AccountId == userId).Select(n => n.CustomerId).FirstOrDefault();

            if (customerId == null || customerId == 0)
            {
                customerId = _context.Employees.Where(n => n.AccountId == userId).Select(n => n.EmployeeId).FirstOrDefault();
            }
            postComment.CustomerId = customerId;
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
