using FanFiction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Services
{
    public interface IChapterRepository

    {
        IEnumerable<Chapter> Chapters{ get; }
        Chapter GetById(string id);


        IEnumerable<Chapter> AddRange(List<Chapter> items);

        public void Delete(Chapter chapter);
    }
}
