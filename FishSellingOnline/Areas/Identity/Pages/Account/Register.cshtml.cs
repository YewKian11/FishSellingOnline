﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FishSellingOnline.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FishSellingOnline.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<FishSellingOnlineUser> _signInManager;
        private readonly UserManager<FishSellingOnlineUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<FishSellingOnlineUser> userManager,
            SignInManager<FishSellingOnlineUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
      
       public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public SelectList RoleselectList = new SelectList
            (
            new List<SelectListItem> {

             new SelectListItem{ Selected = true,Text="Select Role",Value=""},
              new SelectListItem{ Selected = true,Text="Customer",Value="Customer"},
               new SelectListItem{ Selected = true,Text="Seller",Value="Seller"},
                new SelectListItem{ Selected = true,Text="Admin",Value="Admin"}  
            }, "Value","Text",1);
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

 

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage ="Must give your first name before register!")]
            [StringLength(50, ErrorMessage = "Enter your first name with 5-50 chars!", MinimumLength = 5)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Must give your last name before register!")]
            [StringLength(50, ErrorMessage = "Enter your last name with 5-50 chars!", MinimumLength = 5)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Address")]
            [Required(ErrorMessage = "Must give your address before register!")]
            public string Address { get; set; }

            [Display(Name = "Contact Number")]
            [Required(ErrorMessage = "Must give your contact number before register!")]
            public int ContactNumber { get; set; }

            [Display(Name = "Roles")]
            public string userRoles { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new FishSellingOnlineUser { 
                    UserName = Input.Email, 
                    Email = Input.Email, 
                    FirstName = Input.FirstName, 
                    LastName = Input.LastName, 
                    Address = Input.Address, 
                    ContactNumber = Input.ContactNumber
                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                // Line 108 for Customer Register 
               var role =  Roles.Customer.ToString();
                if (Input.userRoles == "Admin") {
                    role = Roles.Admin.ToString();
                }
                if (Input.userRoles == "Seller")
                {
                    role = Roles.Seller.ToString();
                }
                if (result.Succeeded)
                {   // Line 112 Link Role with Customer Register, var in line 97 & 108
                    await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
