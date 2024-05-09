using System;
using System.ComponentModel.DataAnnotations;

namespace Laptop.Areas.Identity.Models.ManageViewModels
{
  public class EditExtraProfileModel
  {
      [Display(Name = "User Name")]
      public string UserName { get; set; }

      [Display(Name = "Email")]
      public string UserEmail { get; set; }
      [Display(Name = "Phone Number")]
      public string PhoneNumber { get; set; }

      [Display(Name = "Address")]
      [StringLength(400)]
      public string HomeAdress { get; set; }


      [Display(Name = "Date Of Birth")]
      public DateTime? BirthDate { get; set; }

      
    }
}