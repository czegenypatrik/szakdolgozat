using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Szakdolgozat.Data.Models;

namespace Szakdolgozat.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Purchases> Purchases { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Todo> Todo { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<Membership> Membership { get; set; }
    }

}
