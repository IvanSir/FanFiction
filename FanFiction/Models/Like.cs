using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class Like
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }

        public string ChapterId { get; set; }

        public bool Liked { get; set; }
    }
}
