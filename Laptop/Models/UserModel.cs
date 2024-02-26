﻿using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace GiayDep.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Vui Lòng Nhập Tài Khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập Địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập Email"), EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage ="Làm ơn nhập Password")]  
        public string Password { get; set; }
    }
}
