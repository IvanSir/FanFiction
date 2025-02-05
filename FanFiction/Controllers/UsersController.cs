﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FanFiction.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FanFiction.Controllers
{
   [Authorize(Roles = "Unblocked")]
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public List<string> IdList { get; set; } = new List<string>();

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> DatabaseAsync()
        {
            if (!await LogoutCheckAsync())
            {
                return View(_userManager.Users.ToList());
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]

        public async Task<ActionResult> Block()
        {
            if (!await LogoutCheckAsync())
            {
                foreach (var id in IdList)
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Unblocked");
                        await _userManager.AddToRoleAsync(user, "Blocked");
                    }
                }
                return RedirectToAction("Database", "Users");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Delete()
        {
            if (!await LogoutCheckAsync())
            {
                foreach (var id in IdList)
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                    }
                }
                return RedirectToAction("Database", "Users");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<ActionResult> Unblock()
        {
            if (!await LogoutCheckAsync())
            {
                foreach (var id in IdList)
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Blocked");
                        await _userManager.AddToRoleAsync(user, "Unblocked");
                    }
                }
                return RedirectToAction("Database", "Users");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<ActionResult> AddAdmin()
        {
            if (!await LogoutCheckAsync())
            {
                foreach (var id in IdList)
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
                return RedirectToAction("Database", "Users");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveAdmin()
        {
            if (!await LogoutCheckAsync())
            {
                foreach (var id in IdList)
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Admin");
                    }

                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Database", "Users");
                    else
                        return RedirectToAction("Index", "Home");
                }

            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<bool> LogoutCheckAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null && await _userManager.IsInRoleAsync(user, "Unblocked"))
                return false;
            return true;
        }
    }
}
