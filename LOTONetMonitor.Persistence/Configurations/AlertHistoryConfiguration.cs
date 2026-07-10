using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for AlertHistory entity
    /// </summary>
    public class AlertHistoryConfiguration : IEntityTypeConfiguration<AlertHistory>
    {
        public void Configure(EntityTypeBuilder<AlertHistory> builder)
        {
            // Table mapping
            builder.ToTable("AlertHistories");

            // Primary key
            builder.HasKey(a => a.AlertID);

            // Properties configuration
            builder.Property(a => a.AlertID)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.AlertType)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("NVARCHAR(20)");

            builder.Property(a => a.PreviousStatus)
                .HasMaxLength(20)
                .HasColumnType("NVARCHAR(20)");

            builder.Property(a => a.CurrentStatus)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("NVARCHAR(20)");

            builder.Property(a => a.Message)
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(a => a.Notes)
                .HasMaxLength(500)
                .HasColumnType("NVARCHAR(500)");

            builder.Property(a => a.EmailSent)
                .HasDefaultValue(false);

            builder.Property(a => a.SMSSent)
                .HasDefaultValue(false);

            builder.Property(a => a.Acknowledged)
                .HasDefaultValue(false);

            builder.Property(a => a.CreatedDate)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.AcknowledgedDate)
                .HasColumnType("DATETIME2");

            builder.Property(a => a.AcknowledgedBy)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR(MAX)");

            // Indexes for performance
            builder.HasIndex(a => a.DeviceID)
                .HasDatabaseName("IX_AlertHistories_DeviceID");

            builder.HasIndex(a => a.CreatedDate)
                .HasDatabaseName("IX_AlertHistories_CreatedDate");

            builder.HasIndex(a => a.CurrentStatus)
                .HasDatabaseName("IX_AlertHistories_CurrentStatus");

            builder.HasIndex(a => a.Acknowledged)
                .HasDatabaseName("IX_AlertHistories_Acknowledged");

            builder.HasIndex(a => new { a.DeviceID, a.CreatedDate })
                .HasDatabaseName("IX_AlertHistories_DeviceID_CreatedDate");

            // Relationships
            builder.HasOne(a => a.Device)
                .WithMany(d => d.AlertHistories)
                .HasForeignKey(a => a.DeviceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
