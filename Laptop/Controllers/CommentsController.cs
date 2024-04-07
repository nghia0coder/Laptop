using Laptop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Laptop.Controllers
{
    public class CommentsController : Controller
    {
        private readonly LaptopContext _context;
        private readonly UserManager<AppUserModel> _userManager;

        public CommentsController(LaptopContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductComment productComment,int rating, string currentUrl)
        {   

            
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
                {
                    return Content("Con cac");
                }
				productComment.CreatedBy = userId;
                productComment.Ratings = rating;
                productComment.CreateDate = DateTime.Now;
                _context.ProductComments.Add(productComment);
                await _context.SaveChangesAsync();
                return Redirect(currentUrl);
         
        }
        public async Task<IActionResult> ByProduct(int productId)
        {
            var comment = await _context.ProductComments
                            .Where(n => n.ProductId == productId && n.Status == true)
                            .ToListAsync();
            return View(comment);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
