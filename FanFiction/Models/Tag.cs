using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class Tag
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string FicId { get; set; }

    }
}
