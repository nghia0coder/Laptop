using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace Laptop.Controllers
{
    public class CompareController : Controller
    {
        private readonly LaptopContext _context;
        private readonly IWebHostEnvironment _webHost;

        public CompareController(LaptopContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }


        public IActionResult Index()
        {
            // Sử dụng LINQ để tạo truy vấn và lấy dữ liệu từ các bảng liên quan
            var products = _context.ProductVariations
                .Include(pv => pv.ProductItems)
                .ThenInclude(pi => pi.Product)
                .Include(pv => pv.Ram) // Kết nối bảng Ram
                .Include(pv => pv.Ssd) // Kết nối bảng Ssd
                .Select(pv => new
                {
                    ProductVarID = pv.ProductVarId,
                    ProductName = $"{pv.ProductItems.Product.ProductName} - {pv.Ram.RamName} - {pv.Ssd.Ssdname}",
                    Imageurl = pv.ProductItems.Image1,
                    Pricepro = $"Current price: {pv.Price.ToString("#,##0")} VNĐ ",
                    Ram = pv.Ram.RamName,
                    Ssd = pv.Ssd.Ssdname,
                    Des = pv.ProductItems.Product.Description,
                    Fea = pv.ProductItems.Product.ProductName,
                    // Kết hợp tên sản phẩm với thông tin Ram và SSD
                })
                .ToList() // Lấy dữ liệu từ cơ sở dữ liệu
                .GroupBy(p => p.ProductName) // Nhóm các mục dựa trên tên sản phẩm
                .Select(g => g.First()) // Chọn một mục duy nhất từ mỗi nhóm
                .ToList(); // Chuyển đổi kết quả thành danh sách

            // Tạo SelectList từ kết quả truy vấn

            SelectList productList = new SelectList(products, "ProductVarID", "ProductName");

            SelectList productsList = new SelectList(products, "ProductVarID", "Imageurl");

            SelectList productssList = new SelectList(products, "ProductVarID", "Pricepro");

            SelectList product1List = new SelectList(products, "ProductVarID", "Ram");

            SelectList product2List = new SelectList(products, "ProductVarID", "Ssd");

            SelectList product3List = new SelectList(products, "ProductVarID", "Des");

            SelectList product4List = new SelectList(products, "ProductVarID", "Fea");

            ViewBag.Images = productsList;

            ViewBag.Product = productList;

            ViewBag.Price = productssList;

            ViewBag.Ram = product1List;

            ViewBag.Ssd = product2List;

            ViewBag.Des = product3List;

            ViewBag.Fea = product4List;

            return View();
        }

		public IActionResult AddtoCompare(int id)
		{
			// Sử dụng LINQ để tạo truy vấn và lấy dữ liệu từ các bảng liên quan
			var products = _context.ProductVariations
				.Include(pv => pv.ProductItems)
				.ThenInclude(pi => pi.Product)
				.Include(pv => pv.Ram) // Kết nối bảng Ram
				.Include(pv => pv.Ssd) // Kết nối bảng Ssd
				.Select(pv => new
				{
					ProductVarID = pv.ProductVarId,
					ProductName = $"{pv.ProductItems.Product.ProductName} - {pv.Ram.RamName} - {pv.Ssd.Ssdname}",
					Imageurl = pv.ProductItems.Image1,
					Pricepro = $"Current price: {pv.Price.ToString("#,##0")} VNĐ ",
                    Ram = pv.Ram.RamName,
                    Ssd = pv.Ssd.Ssdname,
                    Des = pv.ProductItems.Product.Description,
                    Fea = pv.ProductItems.Product.ProductName,
                    // Kết hợp tên sản phẩm với thông tin Ram và SSD
                })
				.ToList() // Lấy dữ liệu từ cơ sở dữ liệu
				.GroupBy(p => p.ProductName) // Nhóm các mục dựa trên tên sản phẩm
				.Select(g => g.First()) // Chọn một mục duy nhất từ mỗi nhóm
				.ToList(); // Chuyển đổi kết quả thành danh sách

            var productadd = _context.ProductVariations
                .Include (pv => pv.ProductItems)
                .ThenInclude (pi => pi.Product)
                .Include(pv => pv.Ram) // Kết nối bảng Ram
                .Include(pv => pv.Ssd) // Kết nối bảng Ssd
                .FirstOrDefault(n => n.ProductVarId == id);

          

			SelectList productList = new SelectList(products, "ProductVarID", "ProductName");

			SelectList productsList = new SelectList(products, "ProductVarID", "Imageurl");

			SelectList productssList = new SelectList(products, "ProductVarID", "Pricepro");

            SelectList product1List = new SelectList(products, "ProductVarID", "Ram");

            SelectList product2List = new SelectList(products, "ProductVarID", "Ssd");

            SelectList product3List = new SelectList(products, "ProductVarID", "Des");

            SelectList product4List = new SelectList(products, "ProductVarID", "Fea");


            ViewBag.Images = productsList;

			ViewBag.Product = productList;

			ViewBag.Price = productssList;

            ViewBag.Ram = product1List;

            ViewBag.Ssd = product2List;

            ViewBag.Des = product3List;

            ViewBag.Fea = product4List;


            return View(productadd);
		}


	}
}
