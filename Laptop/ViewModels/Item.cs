using GiayDep.Models;

namespace GiayDep.ViewModels
{
    public class Item
    {
       public List<CartItemsModel> CartItems { get; set; }
        public int Total { get; set; }
        public int Quanity { get; set; }
    }
}
