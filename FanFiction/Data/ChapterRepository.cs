using FanFiction.Models;
using FanFiction.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Data
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext appDbContext;

        public ChapterRepository(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IEnumerable<Chapter> Chapters => appDbContext
                                                  .Chapters
                                                  .Include(chapter => chapter)
                                                  .AsEnumerable();

        public Chapter GetById(string id)
        {
            return Chapters.Single(fiction => fiction.Id == id);
        }

        public IEnumerable<Chapter> AddRange(List<Chapter> items)
        {
            List<Chapter> chapters = new List<Chapter>();

            foreach (var item in items)
            {
                chapters.Add(appDbContext.Chapters.Add(item).Entity);
                appDbContext.SaveChanges();
            }
            return chapters;
        }

        public void Delete(Chapter chapter)
        {
            appDbContext.Chapters.Remove(chapter);
            appDbContext.SaveChanges();
        }
    }
}
