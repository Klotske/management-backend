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
        
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<Rate> Rates { get; set; }
        
        public DbSet<Schedule> Schedules { get; set; }

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
            
            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasColumnType("nchar(30)")
                    .IsUnicode();
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.PositionId).HasColumnName("PositionId");
                
                entity.Property(e => e.StartDate).HasColumnName("StartDate");

                entity.Property(e => e.Amount).HasColumnName("Amount");

                entity.HasOne(e => e.Position)
                    .WithMany()
                    .HasForeignKey(e => e.PositionId);
            });
            
            modelBuilder.Entity<IntervalRate>(entity =>
            {
                entity.Property(e => e.PositionId).HasColumnName("PositionId");
                
                entity.Property(e => e.StartDate)
                    .HasColumnName("StartDate");
                
                entity.Property(e => e.EndDate)
                    .HasColumnName("EndDate");

                entity.Property(e => e.Amount).HasColumnName("Amount");

                entity.HasOne(e => e.Position)
                    .WithMany()
                    .HasForeignKey(e => e.PositionId);
            });
            
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentId");

                entity.Property(e => e.PositionId).HasColumnName("PositionId");
                
                entity.Property(e => e.StartDate).HasColumnName("StartDate");

                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                entity.HasOne(e => e.Department)
                    .WithMany()
                    .HasForeignKey(e => e.DepartmentId);

                entity.HasOne(e => e.Position)
                    .WithMany()
                    .HasForeignKey(e => e.PositionId);
            });
        }
    }
}
