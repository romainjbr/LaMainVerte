using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class PlantDbContext : DbContext
{
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<WateringLog> WateringLogs => Set<WateringLog>();

    public PlantDbContext(DbContextOptions<PlantDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<WateringLog>()
        .HasOne(w => w.Plant)
        .WithMany(p => p.WateringLogs)
        .HasForeignKey(w => w.PlantId)
        .OnDelete(DeleteBehavior.Cascade);     
    }
}