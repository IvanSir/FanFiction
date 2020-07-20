using FanFiction.Controllers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class ApplicationUser: IdentityUser
    {
        public DateTime LastLoginDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public string CurrentRole { get; set; }

        public List<Fiction> Fictions { get; set; } = new List<Fiction>();

        public List<Rate> idRated { get; set; } = new List<Rate>();

        public async Task<string> GetCurrentRole(UserManager<ApplicationUser> userman)
        {
            var unblocked = await userman.IsInRoleAsync(this, "Unblocked");
            var blocked = await userman.IsInRoleAsync(this, "Blocked");
            var admin = await userman.IsInRoleAsync(this, "Admin");

            if (admin && blocked)
            {
                return "Blocked Admin";
            }

            if (admin && !blocked)
            {
                return "Admin";
            }

            if (unblocked)
                return "Unblocked";
            else return "Blocked";

        }

    }
}
