﻿using Laptop.Models;

namespace Laptop.ViewModels
{
    public class Item
    {
        public List<CartItemsModel> CartItems { get; set; } = null;
        public long Total { get; set; }
        public int Quanity { get; set; }
    }
}
