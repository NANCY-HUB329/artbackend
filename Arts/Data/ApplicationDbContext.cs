using Microsoft.EntityFrameworkCore;
using Arts.Data;
namespace Arts.Data
{
    
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            public DbSet<Art> Arts { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
               

                base.OnModelCreating(modelBuilder);
            }
        }
    }

