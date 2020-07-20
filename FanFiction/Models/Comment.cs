using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public string UserId { get; set; }

        public Chapter Chapter { get; set; }
    }
}
