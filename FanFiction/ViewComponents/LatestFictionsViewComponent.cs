using FanFiction.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.ViewComponents
{
    public class LatestFictionsViewComponent : ViewComponent
    {
 
            private readonly ApplicationDbContext _context;

            public LatestFictionsViewComponent(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IViewComponentResult> InvokeAsync(int howMany)
            {
                var lastFict = await _context.Fictions
                                                .OrderByDescending(a => a.LastModifiedDate)
                                                .Include(a => a.Author)
                                                .Take(howMany)
                                                .ToListAsync();
                return View(lastFict);
            }
        
    }
}
