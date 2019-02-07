using BookStore.Api.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Identity.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}