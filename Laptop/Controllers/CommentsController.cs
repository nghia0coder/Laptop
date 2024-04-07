using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Controllers
{
    public class CommentsController : Controller
    {
        private readonly LaptopContext _context;

        public CommentsController(LaptopContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("CommentId,Name,Email,Detail,Status,ProductId,CreatedBy,CreateDate")] ProductComment productComment)
        {
            if (ModelState.IsValid)
            {
                productComment.CreateDate = DateTime.Now;
                _context.ProductComments.Add(productComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productComment);
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
