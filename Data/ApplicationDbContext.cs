using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcIdentityApp.Models;

namespace MvcIdentityApp.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        internal object products;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        
    }
}
