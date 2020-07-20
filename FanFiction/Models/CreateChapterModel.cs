using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class CreateChapterModel 
    {
        [Required]
        public Chapter Chapter { get; set; }
         public IFormFile Image { get; set; }

    }
}
