namespace Laptop.ViewModels
{
    public class AddressViewModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public bool IsDefault { get; set; }
        public int AddressID { get; set; }
    }
}
