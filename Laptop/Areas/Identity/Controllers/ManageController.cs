// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Laptop.Areas.Identity.Models.ManageViewModels;
using Laptop.Interface;
using Laptop.Models;
using Laptop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace Laptop.Areas.Identity.Controllers
{

    [Authorize]
    [Area("Identity")]
    [Route("/Member/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ManageController> _logger;
        private readonly LaptopContext _context;

        public ManageController(
        UserManager<AppUserModel> userManager,
        SignInManager<AppUserModel> signInManager,
        IEmailSender emailSender,
        ILogger<ManageController> logger,
        LaptopContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Đã thay đổi mật khẩu."
                : message == ManageMessageId.SetPasswordSuccess ? "Đã đặt lại mật khẩu."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "Có lỗi."
                : message == ManageMessageId.AddPhoneSuccess ? "Đã thêm số điện thoại."
                : message == ManageMessageId.RemovePhoneSuccess ? "Đã bỏ số điện thoại."
                : "";

            var user = await GetCurrentUserAsync();
            var customerId = _context.Customers.Where(n => n.AccountId == user.Id).Select(n => n.CustomerId).FirstOrDefault();

            if (customerId == 0)
            {
                customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
            }
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user),
                profile = new EditExtraProfileModel()
                {
                    
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    PhoneNumber = user.PhoneNumber,
                }
            };
            ViewBag.Address = _context.CustomerAddresses.Where(n => n.CustomerId == customerId).OrderByDescending(a => a.IsDefault).ToList();
            return View(model);
        }
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        private Task<AppUserModel> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                ModelState.AddModelError(string.Empty,result.ToString());
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }
        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                ModelState.AddModelError(string.Empty, result.ToString());
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "Đã loại bỏ liên kết tài khoản."
                : message == ManageMessageId.AddLoginSuccess ? "Đã thêm liên kết tài khoản"
                : message == ManageMessageId.Error ? "Có lỗi."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }


        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = result.Succeeded ? ManageMessageId.AddLoginSuccess : ManageMessageId.Error;
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }


        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }
        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _emailSender.SendSmsAsync(model.PhoneNumber, "Mã xác thực là: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }
        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(await GetCurrentUserAsync(), phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Lỗi thêm số điện thoại");
            return View(model);
        }
        //
        // GET: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }
        //
        // POST: /Manage/ResetAuthenticatorKey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticatorKey()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                _logger.LogInformation(1, "User reset authenticator key.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/GenerateRecoveryCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCode()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var codes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5);
                _logger.LogInformation(1, "User generated new recovery code.");
                return View("DisplayRecoveryCodes", new DisplayRecoveryCodesViewModel { Codes = codes });
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfileAsync()
        {
            var user = await GetCurrentUserAsync();
            
            var model = new EditExtraProfileModel()
            {
                
                UserName = user.UserName,
                UserEmail = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfileAsync(EditExtraProfileModel model)
        {
            var user = await GetCurrentUserAsync();

     
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Index), "Manage");

        }
        [HttpPost]
        public async Task<IActionResult> SaveAddress(AddressViewModel add)
        {

         
            var user = await GetCurrentUserAsync();
            var customerId = _context.Customers.Where(n => n.AccountId == user.Id).Select(n => n.CustomerId).FirstOrDefault();

            if (customerId == 0)
            {
                customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
            }

            if (add.IsDefault)
            {
                var addressDefault = _context.CustomerAddresses.Where(n => n.CustomerId == customerId && n.IsDefault == true).FirstOrDefault();

                addressDefault.IsDefault = false;
                _context.CustomerAddresses.Update(addressDefault);
                await _context.SaveChangesAsync();


            }
            var addressNotebook = new AddressNotebook()
            {
                AddressId = add.AddressID
            };

            _context.AddressNotebooks.Add(addressNotebook);
            await _context.SaveChangesAsync();

            var customerAddress = new CustomerAddress()
            {
                CustomerId = customerId,
                AddressNoteId = addressNotebook.AddressNoteId,
                PhoneNumber = add.PhoneNumber,
                Name = add.Name,
                Address = add.Address,
            };

            if (_context.CustomerAddresses.Where(n => n.CustomerId == customerId).ToList().Count() == 0)
            {
                customerAddress.IsDefault = true;
            }
            else
            {
                customerAddress.IsDefault = add.IsDefault;
            }    
            _context.CustomerAddresses.Add(customerAddress);

            TempData["error"] = "Tạo Thành Công";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Manage");

        }


        [HttpPost]
        public async Task<IActionResult> UpdateAddress(int? id,AddressViewModel add)
        {
            var user = await GetCurrentUserAsync();
            var customerId = _context.Customers.Where(n => n.AccountId == user.Id).Select(n => n.CustomerId).FirstOrDefault();

            if (customerId == 0)
            {
                customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
            }

            if(id != null)
            {
                var updateAddress = _context.AddressNotebooks.Where(n => n.AddressNoteId == id).FirstOrDefault();

                updateAddress.AddressId = add.AddressID;

                _context.AddressNotebooks.Update(updateAddress);
                await _context.SaveChangesAsync();
            }    

            if (add.IsDefault)
            {
                var addressDefault = _context.CustomerAddresses.Where(n => n.CustomerId == customerId && n.IsDefault == true).FirstOrDefault();
                if (addressDefault != null)
                {
                    addressDefault.IsDefault = false;
                    _context.CustomerAddresses.Update(addressDefault);
                    await _context.SaveChangesAsync();
                }

            }
           


            var customerAddress = _context.CustomerAddresses.Where(n => n.CustomerId == customerId && n.AddressNoteId == id).FirstOrDefault();

            if(customerAddress == null)
            {
                return Json(id);
            }

            customerAddress.PhoneNumber = add.PhoneNumber;
            customerAddress.Name = add.Name;
            customerAddress.Address = add.Address;
            customerAddress.IsDefault = add.IsDefault;


            if (_context.CustomerAddresses.Where(n => n.CustomerId == customerId).ToList().Count() == 0)
            {
                customerAddress.IsDefault = true;
            }
            else
            {
                customerAddress.IsDefault = add.IsDefault;
            }
            _context.CustomerAddresses.Update(customerAddress);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Manage");

        }
        [HttpGet]
        public async Task<JsonResult> GetProvince()
        {

            var provinces = await _context.Provinces.ToListAsync();

            return Json(provinces);
        }
        [HttpGet]
        public async Task<JsonResult> GetDistrict(int? province) { 
            

         
            
                var districts = await _context.Districts.Where(n => n.ProvinceId == province).ToListAsync();
                return Json(districts);
            
          
        }

        [HttpGet]
        public async Task<JsonResult> GetWard(int? district)
        {
            if(district == null)
            {
                var wards = await _context.Wards.ToListAsync();

                return Json(wards);
            }
            else
            {
                var wards = await _context.Wards.Where(n => n.DistrictId == district).ToListAsync();

                return Json(wards);
            }    
          
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var address = await _context.AddressNotebooks.FindAsync(id);

                if (address != null)
                {
                    _context.Remove(address);
                    await _context.SaveChangesAsync();
                    TempData["error"] = "Xóa Địa Chỉ Thành Công";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Địa chỉ không tồn tại";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log the exception somewhere or handle it appropriately
                TempData["error"] = "Đã xảy ra lỗi khi xóa địa chỉ";
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<JsonResult> GetAddress(int addId)
        {

            var user = await GetCurrentUserAsync();
            var customerId = _context.Customers.Where(n => n.AccountId == user.Id).Select(n => n.CustomerId).FirstOrDefault();

            if (customerId == 0)
            {
                customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
            }

            var getAddress = _context.CustomerAddresses.Where(n => n.CustomerId == customerId && n.AddressNoteId == addId).FirstOrDefault();

            var addNote = await _context.AddressNotebooks.FindAsync(getAddress.AddressNoteId);
            

            var distrinct2  = await _context.Wards.Where(n => n.WardsId == addNote.AddressId).Select(n => n.DistrictId).FirstOrDefaultAsync();

            var province = await _context.Districts.Where(n => n.DistrictId == distrinct2).Select(n => n.ProvinceId).FirstOrDefaultAsync();


            var distrinct = await _context.Wards.FindAsync(addNote.AddressId);

            var getProvince = await _context.Provinces.Where(n => n.ProvinceId == province).FirstOrDefaultAsync();

            var getDistrinct = await _context.Districts.Where(n => n.DistrictId == distrinct2).FirstOrDefaultAsync();





            return Json(new { address = getAddress, city = getProvince, state = getDistrinct});
        }



    }
}
