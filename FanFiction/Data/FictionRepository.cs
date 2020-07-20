using FanFiction.Models;
using FanFiction.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Data
{
    public class FictionRepository : IFictionRepository
    {
        private readonly ApplicationDbContext appDbContext;

        public FictionRepository(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IEnumerable<Fiction> Fictions => appDbContext
                                                  .Fictions
                                                  .Include(fiction => fiction.Author)
                                                  .AsEnumerable();

        public Fiction GetById(string id)
        {
            return Fictions.Single(fiction => fiction.Id == id);
        }

        public Fiction Add(Fiction fiction)
        {
            fiction.UploadDate = DateTime.Now;
            appDbContext.Fictions.Add(fiction);
            appDbContext.SaveChanges();
            return fiction;
        }

        public void Delete(Fiction fiction)
        {
            appDbContext.Fictions.Remove(fiction);
           // appDbContext.SaveChanges();
        }
    }
}
