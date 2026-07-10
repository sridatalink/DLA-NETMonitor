using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Device entity
    /// </summary>
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            // Table mapping
            builder.ToTable("Devices");

            // Primary key
            builder.HasKey(d => d.DeviceID);

            // Properties configuration
            builder.Property(d => d.DeviceID)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.DeviceName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("NVARCHAR(200)");

            builder.Property(d => d.IPAddress)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnType("VARCHAR(45)");

            builder.Property(d => d.Location)
                .HasMaxLength(300)
                .HasColumnType("NVARCHAR(300)");

            builder.Property(d => d.Description)
                .HasMaxLength(500)
                .HasColumnType("NVARCHAR(500)");

            builder.Property(d => d.CurrentStatus)
                .HasMaxLength(20)
                .HasColumnType("NVARCHAR(20)")
                .HasDefaultValue("Unknown");

            builder.Property(d => d.PingIntervalSeconds)
                .HasDefaultValue(30);

            builder.Property(d => d.TimeoutThreshold)
                .HasDefaultValue(3);

            builder.Property(d => d.ResponseTime)
                .HasColumnType("INT");

            builder.Property(d => d.FailureCount)
                .HasDefaultValue(0);

            builder.Property(d => d.CreatedDate)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.UpdatedDate)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);

            // Unique constraint on IP Address
            builder.HasIndex(d => d.IPAddress)
                .IsUnique()
                .HasDatabaseName("IX_Devices_IPAddress_Unique");

            // Indexes for performance
            builder.HasIndex(d => d.CategoryID)
                .HasDatabaseName("IX_Devices_CategoryID");

            builder.HasIndex(d => d.CurrentStatus)
                .HasDatabaseName("IX_Devices_CurrentStatus");

            builder.HasIndex(d => d.IsActive)
                .HasDatabaseName("IX_Devices_IsActive");

            // Relationships
            builder.HasOne(d => d.Category)
                .WithMany(c => c.Devices)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.PingLogs)
                .WithOne(p => p.Device)
                .HasForeignKey(p => p.DeviceID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.AlertHistories)
                .WithOne(a => a.Device)
                .HasForeignKey(a => a.DeviceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
