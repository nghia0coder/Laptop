using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laptop.Models
{
    public partial class Warranty
    {
        public int WarrantyId { get; set; }

        [Required]
        public string OrderId { get; set; }

       // [DataType(DataType.Date)]
        [Required]
        public DateTime? CreateDate { get; set; }

        //[DataType(DataType.Date)]
        [Required]
        public DateTime? EndDate { get; set; }

        public string? Reason { get; set; }
        public string? Service { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }

        public int? ProductId { get; set; }    
        public virtual Order? Order { get; set; }

        
    }
}
