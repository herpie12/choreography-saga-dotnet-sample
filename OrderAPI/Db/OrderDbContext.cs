using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderAPI.Models;
using static Shared.Contracts.OrderCreatedEvent;

namespace OrderAPI.Db
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                     .Property(o => o.status)
                     .HasConversion(new EnumToStringConverter<OrderStatus>());
        }
        public DbSet<Order> Order { get; set; }

        public DbSet<OrderItems> OrderItems { get; set; }
    }
}

