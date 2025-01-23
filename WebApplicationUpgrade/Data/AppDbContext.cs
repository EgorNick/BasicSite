using Microsoft.EntityFrameworkCore;
using WebApplicationUpgrade.Models;

namespace WebApplicationUpgrade.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ContactEntity> Contacts { get; set; }
    }
}