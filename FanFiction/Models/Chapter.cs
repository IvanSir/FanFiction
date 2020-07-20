using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class Chapter
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FictionId { get; set; }

        //[Required]
        //[Display(Name = "Title")]
        public string Title { get; set; }

        //[Required]
        public int ChapterNumber { get; set; }

        public int Rating { get; set; }

        public List<Like> Likers { get; set; } = new List<Like>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

       // [Display(Name = "Image")]
        public string PictureURL { get; set; }



        [Required]

        public string ChapterText { get; set; }
    }
}
