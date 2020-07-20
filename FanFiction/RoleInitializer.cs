using FanFiction.Data;
using FanFiction.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext _context)
        {
            if (await roleManager.FindByNameAsync("Unblocked") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Unblocked"));
            }
            if (await roleManager.FindByNameAsync("Blocked") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Blocked"));
            }
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("Confirmed") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Confirmed"));
            }
           
            if (await _context.Genres.FindAsync("Action") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Action" });
            }
            if (await _context.Genres.FindAsync("Humour") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Humour" });
            }
            if (await _context.Genres.FindAsync("Parody") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Parody" });
            }
            if (await _context.Genres.FindAsync("Darkfic") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Darkfic" });
            }

            await _context.SaveChangesAsync();
        }
    }
}
