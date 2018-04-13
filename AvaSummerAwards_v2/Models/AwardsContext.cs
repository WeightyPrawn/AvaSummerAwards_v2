using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Awards.Models
{
    public class AwardsContext : DbContext
    {
        public AwardsContext(DbContextOptions<AwardsContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Nomination> Nominations { get; set; }
        public DbSet<Nominee> Nominees { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Nomination>().ToTable("Nominations");
            modelBuilder.Entity<Nominee>().ToTable("Nominees");
            modelBuilder.Entity<Vote>().ToTable("Votes");
        }
    }
}