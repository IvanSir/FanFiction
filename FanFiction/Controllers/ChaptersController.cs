using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanFiction.Data;
using FanFiction.Models;
using FanFiction.Services;
using Korzh.EasyQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FanFiction.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IFictionRepository _fictionRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly GoogleDriveRepository googleDriveRepository;
        private readonly string defaultImagePath;

        public ChaptersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IFictionRepository fictionRepository,
            IChapterRepository chapterRepository,
            GoogleDriveRepository googleDriveRepository,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _fictionRepository = fictionRepository;
            _chapterRepository = chapterRepository;
            this.googleDriveRepository = googleDriveRepository;
            defaultImagePath = googleDriveRepository.GetImageLink("1LkV9rOL_nxy314zW6F0V-OKYIYfUeV99");
        }

        public IActionResult Create()
        {

            return View("Create");
        }


       
        [HttpPost]
        
        public async Task<IActionResult> Create(CreateChapterModel model)
        {
            model.Chapter.PictureURL = UploadImage(model.Image);
            var user = await  _userManager.GetUserAsync(User);

                var id = TempData["Fiction"].ToString();
                var fiction = _context.Fictions.Single(i => i.Id == id);

                fiction.Chapters = _context.Chapters.Where(chap => chap.FictionId == id).ToList();
                fiction.LastModifiedDate = DateTime.Now;


                model.Chapter.FictionId = fiction.Id;

                var chapterCounter = fiction.Chapters.Count;

                model.Chapter.ChapterNumber += chapterCounter;
                model.Chapter.Rating = 0;

                

                fiction.LastModifiedDate = DateTime.Now;

                fiction.Chapters.Add(model.Chapter);

               

                _chapterRepository.Chapters.Append(model.Chapter);
                
                await _context.SaveChangesAsync();
            
            return RedirectToAction("Chapter", new { Id = model.Chapter.Id });
        }

        [HttpPost]
        public async Task<ActionResult> LikeAsync(string id)
        {
            var liked = _userManager.GetUserAsync(User).Result;

            var chapter = _chapterRepository.GetById(id);
            
            chapter.Likers = _context.Likes.Where(like => like.ChapterId == id).ToList();

            if (chapter.Likers.Where(like => like.UserId == liked.Id && like.Liked == true).Count() == 0)
            {

                var like = new Like()
                {
                    Liked = true,
                    ChapterId = id,
                    UserId = liked.Id,
                };

                chapter.Likers.Add(like);
                chapter.Rating++;

                _context.Likes.Add(like);
            }

            
            await _context.SaveChangesAsync();
            return RedirectToAction("Chapter", new { Id = chapter.Id });


        }

        [HttpPost]
        public async Task<ActionResult> DisLikeAsync(string id)
        {
            var user = _userManager.GetUserAsync(User).Result;

            var chapter = _chapterRepository.GetById(id);

            chapter.Likers = _context.Likes.Where(like => like.ChapterId == id).ToList();

            if (chapter.Likers.Where(like => like.UserId == user.Id && like.Liked == true).Count() != 0)
            {

                var like = _context.Likes.Single(like => like.UserId == user.Id && like.ChapterId == chapter.Id);


                chapter.Likers.Remove(like);
                chapter.Rating--;

                _context.Likes.Remove(like);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction("Chapter", new { Id = chapter.Id });


        }

        [Route("~/chapters/id={id}")]
        public ViewResult Chapter(string id)
        {
            var chapter = _chapterRepository.GetById(id);

            chapter.Likers = _context.Likes.Where(like => like.ChapterId == id).ToList();
            chapter.Comments = _context.Comments.Where(com => com.Chapter == chapter).ToList();

            return View(chapter);
        }

        public RedirectToActionResult ChapterbyNam(string chapNam, string id)
        {

            var chapter = _chapterRepository.Chapters.Single(chapa=>chapa.ChapterNumber == chapNam.ToInt() && chapa.Id == id);
      

            return RedirectToAction("Chapter", new { Id = chapter.Id });
        }


        [HttpPost]
        public async Task<IActionResult> AddCommentAsync(Comment comment, string chapterId)
        {
            var author = _userManager.GetUserAsync(User).Result;
            comment.CommentDate = DateTime.Now;
            comment.UserId = author.Id;
            _context.Chapters.Find(chapterId).Comments.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Chapter", new { Id = chapterId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommentAsync(string commentId,string chapterId)
        {
             var comment = _context.Comments.Find(commentId);

            _context.Chapters.Find(chapterId).Comments.Remove(comment);
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Chapter", new { Id = chapterId });
        }


        private string UploadImage(IFormFile imageFile)
        {
            string imagePath = defaultImagePath;

            if (imageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + imageFile.FileName;
                imagePath = googleDriveRepository
                    .GetImageLink(googleDriveRepository
                    .UploadFIle(fileName, imageFile.OpenReadStream()));
            }
            return imagePath;
        }


    }
}
