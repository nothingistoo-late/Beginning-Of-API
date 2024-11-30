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
    }
}
