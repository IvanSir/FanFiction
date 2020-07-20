using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FanFiction.Data;
using FanFiction.Models;
using FanFiction.Services;
using Korzh.EasyQuery.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Logging;

namespace FanFiction.Controllers
{
    public class FictionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IFictionRepository _fictionRepository;
        private readonly IChapterRepository _chapterRepository;

        public FictionsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IFictionRepository fictionRepository,
            IChapterRepository chapterRepository,
        ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _fictionRepository = fictionRepository;
            _chapterRepository = chapterRepository;
        }

        [BindProperty]
        public List<string> IdList { get; set; } = new List<string>();




        public IActionResult Create()
        {
            return View();
        }
        public ViewResult List()
        {
            FictionList viewModel = new FictionList
            {
                Fictions = _fictionRepository.Fictions
            };
            return View(viewModel);
        }

        public IActionResult CreateByAdmin()
        {
            return View("Create");
        }


        [Authorize]
        [HttpPost]

        public async Task<IActionResult> Create(Fiction fiction, string tagName)
        {

            var author = await _userManager.GetUserAsync(User);

            fiction.LastModifiedDate = DateTime.Now;

            fiction.UploadDate = DateTime.Now;

            fiction.Author = author;

            var tag = new Tag()
            {
                Name = tagName,
                FicId = fiction.Id,
            };

            _context.Tags.Add(tag);

            fiction.Tags.Add(tag);

            _fictionRepository.Add(fiction);
            //await _context.SaveChangesAsync();

            author.Fictions.Add(fiction);

            return RedirectToAction("Fiction", new { Id = fiction.Id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateByAdmin(Fiction fiction,string userId, string tagName)
        {

            var author = await _userManager.FindByIdAsync(userId);

            fiction.LastModifiedDate = DateTime.Now;

            fiction.UploadDate = DateTime.Now;

            fiction.Author = author;

            var tag = new Tag()
            {
                Name = tagName,
                FicId = fiction.Id,
            };

            _context.Tags.Add(tag);

            fiction.Tags.Add(tag);

            _fictionRepository.Add(fiction);
            //await _context.SaveChangesAsync();

            author.Fictions.Add(fiction);

            return RedirectToAction("Fiction", new { Id = fiction.Id });
        }


        [HttpPost]
        public async Task<ActionResult> Delete()
        {
            var author = await _userManager.GetUserAsync(User);

            foreach (var id in IdList)
            {
                var fiction = _fictionRepository.GetById(id);
           

                var chapters = _chapterRepository.Chapters.Where(chap => chap.FictionId == id).ToList();

                var tags = _context.Tags.Where(tag => tag.FicId == fiction.Id).ToList();

                

                _context.Tags.RemoveRange(tags);


                foreach (var ch in chapters)
                {
                    var likes = _context.Likes.Where(like => like.ChapterId == ch.Id).ToList();

                    var coms = _context.Comments.Where(com => com.Chapter == ch).ToList();

                    _context.Comments.RemoveRange(coms);

                    _context.Likes.RemoveRange(likes);

                    fiction.Chapters.Remove(ch);

                    _chapterRepository.Delete(ch);

                }
                var rates = _context.Rates.Where(rata => rata.FictionId == fiction.Id);

                foreach (var rt in rates)
                {
                    _context.Rates.Remove(rt);
                   // _context.SaveChanges();
                }

                _fictionRepository.Delete(fiction);
                fiction.Author.Fictions.Remove(fiction);
                //_context.Fictions.Remove(fiction);

                await _context.SaveChangesAsync();

            }
            return RedirectToAction(author.Id.ToString(), "user");

        }


      [HttpPost]
        public async Task<ActionResult> UprankAsync(string id)
        {
            var user = await _userManager.GetUserAsync(User);

           
            var fiction = _fictionRepository.GetById(id);
           
                fiction.Rating += 1;

                fiction.Author.Fictions.Select(fic => fic.Id == id);
                await _context.SaveChangesAsync();
 
            return RedirectToAction("Fiction", new { Id = fiction.Id });


        }

        [HttpPost]
        public async Task<ActionResult> RateFictionAsync(string fictionId, string rate)
        {

            var newRate = Convert.ToInt32(rate);
            var fiction = _fictionRepository.GetById(fictionId);

            var user = _userManager.GetUserAsync(User).Result;

            user.idRated = _context.Rates.Where(rate => rate.UserId == user.Id && rate.FictionId == fiction.Id).ToList();

            if (user.idRated.Count() == 0)
            {
                var ficRate = new Rate()
                {
                    FictionId = fiction.Id,
                    UserId = user.Id,
                    CurrentRate = newRate,
                };

                user.idRated.Add(ficRate);

                fiction.Rating = (double)(((fiction.Rating * fiction.RatesCount++) + newRate) / fiction.RatesCount);
                _context.Rates.Add(ficRate);
               // await _context.SaveChanges();
            }
            else if (user.idRated.Count() == 1)
            {
                var previousRate = _context.Rates.Single(rata => rata.FictionId == fiction.Id && rata.UserId == user.Id);
                
                _context.Rates.Remove(previousRate);

                var ficRate = new Rate()
                {
                    FictionId = fiction.Id,
                    UserId = user.Id,
                    CurrentRate = newRate,
                };

                _context.Rates.Add(ficRate);
                // 4.5

                fiction.Rating = (double)((fiction.Rating * fiction.RatesCount - previousRate.CurrentRate + newRate)/fiction.RatesCount);
               // fiction.Rating = (double)(((fiction.Rating * fiction.RatesCount++) + newRate) / fiction.RatesCount);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Fiction", new { Id = fiction.Id });


        }




        [HttpPost]
        public async Task<ActionResult> DownrankAsync(string id)
        {
            var liked = await _userManager.GetUserAsync(User);
            var fiction = _fictionRepository.GetById(id);
            fiction.Rating -= 1;

            fiction.Author.Fictions.Select(fic => fic.Id == id);
            await _context.SaveChangesAsync();
            return RedirectToAction("Fiction", new { Id = fiction.Id });


        }


        //[HttpPost]
        //public async Task<ActionResult> DeleteAllFic()
        //{
        //    var author = await _userManager.GetUserAsync(User);

        //    foreach (var id in IdList)
        //    {

        //        var fiction = _fictionRepository.GetById(id);
        //        _fictionRepository.Delete(fiction);
        //       if(fiction.Author!=null)fiction.Author.Fictions.Remove(fiction);
        //    }
        //    return RedirectToAction(author.Id.ToString(), "user");

        //}


        [Route("~/fictions/id={id}")]
        public ViewResult Fiction(string id)
        {
           
           
            var fiction = _fictionRepository.GetById(id);

            TempData["Fiction"] = fiction.Id;

            //fiction.Chapters = _chapterRepository.Chapters.Where(chap => chap.FictionId == id).ToList();

            //var user = _userManager.GetUserAsync(User).Result;

            //user.idRated = _context.Rates.Where(rate => rate.Id == id).ToList();

            //_context.SaveChanges();

            //fiction.Tags = _context.Tags.Where(tag => tag.Id == fiction.Id).ToList();

            return View(_context.Fictions
                .Include(i => i.Chapters).Include(i => i.Tags)
                .Single(i => i.Id == id));
        }

        [HttpGet]
        public IActionResult Search(string words)
        {
            var model = new SearchModel()
            {
                Words = words
            };
            if (string.IsNullOrEmpty(words))
            {
                model.Fictions = _fictionRepository.Fictions;
            }
            else
            {
                model.Fictions = _fictionRepository.Fictions
                    .FullTextSearchQuery(words, null).AsEnumerable();
            }

            return View(model);
        }



    }
}
