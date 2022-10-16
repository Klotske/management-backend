using management_api.Models;
using Microsoft.EntityFrameworkCore;

namespace management_api.Data
{
    public class MSSQLContext: DbContext
    {
        public MSSQLContext()
        {
            Database.EnsureCreated();
        }

        public MSSQLContext(DbContextOptions<MSSQLContext> options)
            :base(options)
        {
        }
        
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Title)
                    .HasColumnName("Title")
                    .HasColumnType("nchar(30)")
                    .IsUnicode();
            });
        }
    }
}
