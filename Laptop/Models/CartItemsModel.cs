namespace Laptop.Models
{
    public class CartItemsModel
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int? SoLuong { get; set; }
        public int? DonGia { get; set; }
        public int? ThanhTien
        {
            get { return DonGia * SoLuong; }
        }
        
        public string HinhAnh { get; set; }

        public CartItemsModel() { }

        // Constructor theo id (dùng cho trường hợp chỉ có sl=1)
        public CartItemsModel(SanPham sanPham)
        {
            MaSP = sanPham.Idsp;
            TenSP = sanPham.Tensp;
            SoLuong = sanPham.Soluong;
            DonGia = sanPham.Dongia;
            SoLuong = 1;
            HinhAnh = sanPham.Hinhanh1;
        }
    }
}
