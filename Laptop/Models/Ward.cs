using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Ward
    {
        public Ward()
        {

        }

        public int WardsId { get; set; }
        public int DistrictId { get; set; }
        public string Name { get; set; } = null!;


    }
}
