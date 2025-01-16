using Final4.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Final4.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions options): base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Flower> Flowers { get; set; }   
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Rating> Ratings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Đặt khóa chính tổng hợp cho bảng OrderDetail (kết hợp OrderId và FlowerId)
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.FlowerId });

            // Cấu hình mối quan hệ giữa OrderDetail và Order
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            //// Cấu hình mối quan hệ giữa OrderDetail và Flower
            //modelBuilder.Entity<OrderDetail>()
            //    .HasOne(od => od.Flower)
            //    .WithMany(f => f.OrderDetail)
            //    .HasForeignKey(od => od.FlowerId);

        }

    }
}
