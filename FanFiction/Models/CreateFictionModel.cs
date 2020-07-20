using FanFiction.Areas.Identity.Pages.Account;
using FanFiction.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class CreateFictionModel: PageModel
    {
            [Required]
            public Fiction Fiction { get; set; }
           
       



    }
}
