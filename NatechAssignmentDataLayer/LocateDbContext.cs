using Microsoft.EntityFrameworkCore;
using NatechAssignmentDataLayer.Models;

namespace NatechAssignmentDataLayer
{
    public class LocateDbContext : DbContext
    {
        public LocateDbContext(DbContextOptions<LocateDbContext> options) : base(options)
        {
        }

        public DbSet<LocateHeader> Headers { get; set; }
        public DbSet<LocateDetails> Details { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocateHeader>().ToTable("Headers");
            modelBuilder.Entity<LocateDetails>().ToTable("Details");
        }
    }
}
