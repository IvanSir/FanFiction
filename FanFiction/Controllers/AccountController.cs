using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanFiction.Data;
using FanFiction.Models;
using FanFiction.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FanFiction.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            //FictionList home = new FictionList()
            //{
            //    Fictions = _fictionRepository.Fictions.OrderByDescending(fiction => fiction.UploadDate)
            //};

            return View();
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IFictionRepository _fictionRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            ApplicationDbContext context,
            IFictionRepository fictionRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _fictionRepository = fictionRepository;
        }

            [TempData]
            public string ErrorMessage { get; set; }

        [Route("~/user/{id}")]
        public IActionResult UserAccount(string id)
        {
            var viewModel = new UserAccount
            {
                UserToShow = GetById(id)
            };



            viewModel.UserToShow.Fictions = _fictionRepository.Fictions.Where(fiction => fiction.Author == viewModel.UserToShow).ToList();

            //FictionList home = new FictionList()
            //{
            //    Fictions = _fictionRepository.Fictions.OrderByDescending(fiction => fiction.UploadDate)
            //};
            //viewModel.UserToShow.Fictions = home.Fictions

            if ((_userManager.GetUserId(User) == viewModel.UserToShow.Id))
            {
                viewModel.IsCurrentUser = true;
            }
            else
            {
                viewModel.IsCurrentUser = false;
            }

            return View(viewModel);
        }

        public ApplicationUser GetById(string id) => _userManager.FindByIdAsync(id).Result;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }


    }
}
