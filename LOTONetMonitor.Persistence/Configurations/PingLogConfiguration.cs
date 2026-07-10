using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for PingLog entity
    /// </summary>
    public class PingLogConfiguration : IEntityTypeConfiguration<PingLog>
    {
        public void Configure(EntityTypeBuilder<PingLog> builder)
        {
            // Table mapping
            builder.ToTable("PingLogs");

            // Primary key
            builder.HasKey(p => p.LogID);

            // Properties configuration
            builder.Property(p => p.LogID)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PingTime)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.ResponseTime)
                .HasColumnType("INT");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("NVARCHAR(20)");

            builder.Property(p => p.CreatedDate)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes for performance
            builder.HasIndex(p => p.DeviceID)
                .HasDatabaseName("IX_PingLogs_DeviceID");

            builder.HasIndex(p => p.PingTime)
                .HasDatabaseName("IX_PingLogs_PingTime");

            builder.HasIndex(p => new { p.DeviceID, p.PingTime })
                .HasDatabaseName("IX_PingLogs_DeviceID_PingTime");

            // Relationships
            builder.HasOne(p => p.Device)
                .WithMany(d => d.PingLogs)
                .HasForeignKey(p => p.DeviceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
