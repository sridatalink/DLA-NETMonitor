using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for ApplicationUser entity
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Properties configuration
            builder.Property(u => u.FullName)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR(256)");

            builder.Property(u => u.MobileNumber)
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)");

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(u => u.IsActive)
                .HasDatabaseName("IX_AspNetUsers_IsActive");
        }
    }
}
