using ConstructionSite.Services.Activity.Domain.Entities;
using ConstructionSite.Services.Activity.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;

namespace ConstructionSite.Services.Activity.Infrastructure.Data;

public class ActivityDbContext(DbContextOptions<ActivityDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.Activity> Activities { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ulidConverter = new UlidToStringConverter();

        modelBuilder.Entity<Domain.Entities.Activity>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasConversion(ulidConverter);
            entity.Property(a => a.ActivityTypeId).HasConversion(ulidConverter);

            entity.HasOne(a => a.ActivityType)
                  .WithMany()
                  .HasForeignKey(a => a.ActivityTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ActivityType>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasConversion(ulidConverter);
        });

        base.OnModelCreating(modelBuilder);
    }
}
