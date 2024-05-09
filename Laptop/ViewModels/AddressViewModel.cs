using System.ComponentModel.DataAnnotations;

namespace Laptop.ViewModels
{
    public class AddressViewModel
    {
        [Required(ErrorMessage = "Vui Lòng Nhập Họ Tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập Số Điện Thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập Thông Tin")]
        public string Address { get; set; }

        public bool IsDefault { get; set; }
        public int AddressID { get; set; }
    }
}
