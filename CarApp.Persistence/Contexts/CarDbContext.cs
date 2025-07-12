using CarApp.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CarApp.Persistence.Contexts;

/// <summary>
/// EF Core DbContext for CarApp.
/// </summary>
public sealed class CarDbContext : DbContext
{
    public DbSet<Car> Cars { get; set; } = null!;

    public CarDbContext(DbContextOptions<CarDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var carIdConverter = new CarIdConverter();

        modelBuilder.Entity<Car>(builder =>
        {
            builder.HasKey(c => c.Id);

            // Configure EF to use the converter for the Id property
            builder.Property(c => c.Id)
                   .HasConversion(carIdConverter);

            builder.Property(c => c.Brand).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Model).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Color).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Year).IsRequired();
        });
    }
}
