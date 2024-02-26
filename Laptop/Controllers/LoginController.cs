using GiayDep.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace GiayDep.Controllers
{
    public class LoginController : Controller
    {
        private readonly LaptopContext _context;
        public LoginController(LaptopContext context)
        {
            _context = context;
        }
        public IActionResult DangNhap(IFormCollection f)
        {
            string taikhoan = f["txtTenDangNhap"].ToString();
            string matKhau = f["txtMatKhau"].ToString();

            Membership tv = _context.Memberships.SingleOrDefault(n => n.Tk == taikhoan && n.Mk == matKhau);

            if (tv != null)
            {
                var lstQuyen = _context.MembershipRights
                    .Include(n => n.IdrightNavigation)
                    .Where(n => n.MaLoaiTv == tv.MaLoaiTv);

                if (lstQuyen.Any())
                {
                    string Quyen = string.Join(",", lstQuyen.Select(item => item.IdrightNavigation.Idright));
                  
                    PhanQuyen(tv.Tk.ToString(), Quyen);
                    HttpContext.Session.SetString("TaiKhoan", Newtonsoft.Json.JsonConvert.SerializeObject(tv));
                    return RedirectToAction("Index", "Home");
                }
            }
            return Content("Tài khoản hoặc mật khẩu không chính xác.");
        }

        private void PhanQuyen(string Taikhoan, string Quyen)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Taikhoan),
            new Claim(ClaimTypes.Role, Quyen)
        };

            var identity = new ClaimsIdentity(claims, "cookie");
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync("cookie", principal);
        }

    }
}
