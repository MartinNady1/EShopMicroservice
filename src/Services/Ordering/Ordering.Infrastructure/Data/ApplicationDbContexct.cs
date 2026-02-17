using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System.Reflection.Emit;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDbContexct : DbContext , IApplicationDbContext
    {
        public ApplicationDbContexct(DbContextOptions<ApplicationDbContexct> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContexct).Assembly);
          
            base.OnModelCreating(builder);


        }


    }
}
