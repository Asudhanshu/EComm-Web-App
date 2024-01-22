using Microsoft.EntityFrameworkCore;
using System;
namespace DataAccessLayer
{
    public class ECommDbContext : DbContext
    {
        public virtual DbSet<Product> product { get; set; }
        public virtual DbSet<Category> category { get; set; }
        public virtual DbSet<Users> users { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<Order> order { get; set; }
        public ECommDbContext(DbContextOptions<ECommDbContext> options) : base(options)
        {

        }
    }
}
