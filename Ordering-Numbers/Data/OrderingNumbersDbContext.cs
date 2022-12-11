using Microsoft.EntityFrameworkCore;
using Ordering_Numbers.Models;

namespace Ordering_Numbers.Data
{
    public class OrderingNumbersDbContext : DbContext
    {
        public OrderingNumbersDbContext(DbContextOptions<OrderingNumbersDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NumbersList>().HasData(new NumbersList());
            modelBuilder.Entity<Number>().HasData(new Number());
        }

        public DbSet<NumbersList> NumbersLists { get; set; }
        public DbSet<Number> Numbers { get; set; }

    }
}
