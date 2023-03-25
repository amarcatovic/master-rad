using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence
{
    public class InMemoryPostCacheDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public InMemoryPostCacheDbContext(DbContextOptions<InMemoryPostCacheDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(e => e.Id)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);
        }
    }
}
