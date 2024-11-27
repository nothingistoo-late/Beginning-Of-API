using Final4.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Final4.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { 
        }
        public DbSet<Employee> Employee { get; set; }

        
    }
}
