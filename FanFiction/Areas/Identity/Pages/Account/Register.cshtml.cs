using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using FanFiction.Models;
using FanFiction.Services;
using FanFiction.Controllers;

namespace FanFiction.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
        ILogger<RegisterModel> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
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
                var user = new ApplicationUser
                {
                    UserName = Input.Name,
                    Email = Input.Email,
                    RegisterDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    CurrentRole = "Unblocked"
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, "Unblocked");
                if (result.Succeeded)
                {
                    #region Email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailSender emailService = new EmailSender();
                    await emailService.SendEmailAsync(user.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Тыкай сюда</a>");

                    return RedirectToAction("EmailConfirmation", "Home");
                    #endregion

                    //_logger.LogInformation("User created a new account with password.");
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        #region Email

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return Redirect(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        //public async Task<IActionResult> SendVerificationEmail()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }

        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
        //    var email = user.Email;
        //    await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

        //    return RedirectToAction(nameof(Index));
        //}
        #endregion

    }
}
