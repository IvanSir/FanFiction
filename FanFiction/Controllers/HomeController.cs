using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FanFiction.Models;
using Microsoft.AspNetCore.Http;
using FanFiction.Data;
using FanFiction.Services;

namespace FanFiction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFictionRepository _fictionRepository;

        public HomeController(ILogger<HomeController> logger, IFictionRepository fictionRepository)
        {
            _logger = logger;
            _fictionRepository = fictionRepository;
        }

        public ViewResult Index()
        {
            FictionList home = new FictionList()
            {
                Fictions = _fictionRepository.Fictions.OrderByDescending(fiction => fiction.UploadDate)
            };
            return View(home);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult EmailConfirmation()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetTheme(string theme)
        {
            Response.Cookies.Append("theme", theme, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
            return Ok();
        }
    }
}
