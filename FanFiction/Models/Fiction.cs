using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class Fiction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public double Rating { get; set; } = 0;

        public int RatesCount { get; set; } = 0;
        public DateTime UploadDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public ApplicationUser Author { get; set; }

        public string Genre { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public List<Chapter> Chapters { get; set; } = new List<Chapter>();

        public async Task<string> GetRating(UserManager<ApplicationUser> userman)
        {
            var unblocked = await userman.IsInRoleAsync(Author, "Unblocked");
            return Rating.ToString();
        }





    }





}
