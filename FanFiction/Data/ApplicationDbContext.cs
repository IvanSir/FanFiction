using System;
using System.Collections.Generic;
using System.Text;
using FanFiction.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FanFiction.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Fiction> Fictions { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
