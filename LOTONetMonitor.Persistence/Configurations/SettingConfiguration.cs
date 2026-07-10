using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Setting entity
    /// </summary>
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            // Table mapping
            builder.ToTable("Settings");

            // Primary key
            builder.HasKey(s => s.SettingID);

            // Properties configuration
            builder.Property(s => s.SettingID)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.SettingKey)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR(100)");

            builder.Property(s => s.SettingValue)
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(s => s.Description)
                .HasMaxLength(500)
                .HasColumnType("NVARCHAR(500)");

            builder.Property(s => s.SettingType)
                .HasMaxLength(50)
                .HasColumnType("NVARCHAR(50)")
                .HasDefaultValue("String");

            builder.Property(s => s.UpdatedDate)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETUTCDATE()");

            // Unique constraint on SettingKey
            builder.HasIndex(s => s.SettingKey)
                .IsUnique()
                .HasDatabaseName("IX_Settings_SettingKey_Unique");
        }
    }
}
