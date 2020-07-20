using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class Genre
    {
        [Key]
        public string GenreName { get; set; }
    }
}
