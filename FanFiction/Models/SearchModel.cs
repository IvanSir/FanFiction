using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class SearchModel
    {
        public IEnumerable<Fiction> Fictions { get; set; }

        public string Words { set; get; }
    }
}
