using Laptop.Models;
using GiayDep.Repository;
using GiayDep.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Laptop.Components
{

    public class CartViewComponent : ViewComponent
    {
        private readonly LaptopContext _context;
        public CartViewComponent(LaptopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<CartItemsModel> cartItems = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();
            Item cart = new()
            {
                CartItems = cartItems,
                Quanity = cartItems.Count(),
                Total = cartItems.Sum(x => (x.SoLuong ?? 0) * (x.DonGia ?? 0))
            };
            ViewBag.TongTien = cart.Total;
            ViewBag.TongSoLuong = cart.Quanity;
            return View();
        }
        public double TinhTongSoLuong()
        {

            List<Item> lstGioHang = LayGioHang();
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.Quanity);
        }
        public List<Item> LayGioHang()
        {

            List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart");
            if (cart == null)
            {

                cart = new List<Item>();
                HttpContext.Session.SetJson("Cart", cart);
            }
            return cart;
        }
        public int? TinhTongTien()
        {
            List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart");
            if (cart == null)
            {
                return 0;
            }
            return cart.Sum(n => n.Total);
        }
    }
}
