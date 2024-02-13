using BidService.Models;
using Microsoft.EntityFrameworkCore;

namespace BidService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Bid> Bids { get; set; }
    }
}
