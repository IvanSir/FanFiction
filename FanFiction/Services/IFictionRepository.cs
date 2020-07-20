using FanFiction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Services
{
    public interface IFictionRepository
    {
        IEnumerable<Fiction> Fictions { get; }
        Fiction GetById(string id);
        Fiction Add(Fiction fiction);

        void Delete(Fiction fiction);
    }
}
