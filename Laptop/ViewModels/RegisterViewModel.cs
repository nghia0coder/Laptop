using System.ComponentModel;

namespace Laptop.ViewModels
{
    public class RegisterViewModel
    {
        [DisplayName("Email")]   
        
        public string Email { get; set; }
        [DisplayName("Name Account")]
        public string UserName { get; set; }
        [DisplayName("User Name")]
        public string? Name { get; set; }
        [DisplayName("Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
    }
}
