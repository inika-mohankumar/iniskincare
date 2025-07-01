using Microsoft.EntityFrameworkCore;
using iniskincare.Models;

namespace iniskincare.Data
{
    public class IniskincareDbContext: DbContext
    {
        public IniskincareDbContext(DbContextOptions<IniskincareDbContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }

}
