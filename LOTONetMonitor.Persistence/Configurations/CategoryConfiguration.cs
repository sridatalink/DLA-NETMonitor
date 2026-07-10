using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Category entity
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table mapping
            builder.ToTable("Categories");

            // Primary key
            builder.HasKey(c => c.CategoryID);

            // Properties configuration
            builder.Property(c => c.CategoryID)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR(100)");

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .HasColumnType("NVARCHAR(500)");

            builder.Property(c => c.CreatedDate)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.UpdatedDate)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            // Unique constraint
            builder.HasIndex(c => c.CategoryName)
                .IsUnique()
                .HasDatabaseName("IX_Categories_CategoryName_Unique");

            // Relationships
            builder.HasMany(c => c.Devices)
                .WithOne(d => d.Category)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
