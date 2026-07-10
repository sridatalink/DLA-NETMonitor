using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Persistence.Configurations;

namespace LOTONetMonitor.Persistence.Data
{
    /// <summary>
    /// Application database context using SQL Server and Entity Framework Core
    /// Inherits from IdentityDbContext for ASP.NET Identity support
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for Categories
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// DbSet for Devices
        /// </summary>
        public DbSet<Device> Devices { get; set; }

        /// <summary>
        /// DbSet for Ping Logs
        /// </summary>
        public DbSet<PingLog> PingLogs { get; set; }

        /// <summary>
        /// DbSet for Alert History
        /// </summary>
        public DbSet<AlertHistory> AlertHistories { get; set; }

        /// <summary>
        /// DbSet for Application Settings
        /// </summary>
        public DbSet<Setting> Settings { get; set; }

        /// <summary>
        /// Configure the model and apply entity configurations
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new PingLogConfiguration());
            modelBuilder.ApplyConfiguration(new AlertHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());

            // Seed initial data
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Seed initial data for categories and settings
        /// </summary>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Data Center", Description = "Data center infrastructure" },
                new Category { CategoryID = 2, CategoryName = "Servers", Description = "Server machines and hosts" },
                new Category { CategoryID = 3, CategoryName = "Routers", Description = "Network routers" },
                new Category { CategoryID = 4, CategoryName = "Switches", Description = "Network switches" },
                new Category { CategoryID = 5, CategoryName = "Internet", Description = "Internet connectivity devices" },
                new Category { CategoryID = 6, CategoryName = "Firewalls", Description = "Firewall appliances" },
                new Category { CategoryID = 7, CategoryName = "Branches", Description = "Branch office devices" },
                new Category { CategoryID = 8, CategoryName = "Lottery Terminals", Description = "Lottery terminal machines" },
                new Category { CategoryID = 9, CategoryName = "POS", Description = "Point of Sale terminals" }
            );

            // Seed Default Settings
            modelBuilder.Entity<Setting>().HasData(
                new Setting
                {
                    SettingID = 1,
                    SettingKey = "SMTPServer",
                    SettingValue = "smtp.gmail.com",
                    Description = "SMTP server address for email alerts",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 2,
                    SettingKey = "SMTPPort",
                    SettingValue = "587",
                    Description = "SMTP server port",
                    SettingType = "Int"
                },
                new Setting
                {
                    SettingID = 3,
                    SettingKey = "SMTPUsername",
                    SettingValue = "",
                    Description = "SMTP username for authentication",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 4,
                    SettingKey = "SMTPPassword",
                    SettingValue = "",
                    Description = "SMTP password (encrypted)",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 5,
                    SettingKey = "TwilioAccountSID",
                    SettingValue = "",
                    Description = "Twilio Account SID for SMS service",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 6,
                    SettingKey = "TwilioAuthToken",
                    SettingValue = "",
                    Description = "Twilio Auth Token for SMS service",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 7,
                    SettingKey = "TwilioPhoneNumber",
                    SettingValue = "",
                    Description = "Twilio phone number for SMS alerts",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 8,
                    SettingKey = "CompanyName",
                    SettingValue = "LOTO NET Monitor",
                    Description = "Company name for email signatures",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 9,
                    SettingKey = "DefaultRefreshInterval",
                    SettingValue = "30",
                    Description = "Dashboard refresh interval in seconds",
                    SettingType = "Int"
                },
                new Setting
                {
                    SettingID = 10,
                    SettingKey = "DefaultTimeoutCount",
                    SettingValue = "3",
                    Description = "Default failure count before marking offline",
                    SettingType = "Int"
                },
                new Setting
                {
                    SettingID = 11,
                    SettingKey = "DefaultEmail",
                    SettingValue = "",
                    Description = "Default email address for alerts",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 12,
                    SettingKey = "DefaultMobileNumber",
                    SettingValue = "",
                    Description = "Default mobile number for SMS alerts",
                    SettingType = "String"
                },
                new Setting
                {
                    SettingID = 13,
                    SettingKey = "PingTimeout",
                    SettingValue = "5000",
                    Description = "Ping timeout in milliseconds",
                    SettingType = "Int"
                },
                new Setting
                {
                    SettingID = 14,
                    SettingKey = "WarningThreshold",
                    SettingValue = "200",
                    Description = "Response time threshold for warning status (ms)",
                    SettingType = "Int"
                },
                new Setting
                {
                    SettingID = 15,
                    SettingKey = "EnableEmailAlerts",
                    SettingValue = "true",
                    Description = "Enable/disable email alerts globally",
                    SettingType = "Bool"
                },
                new Setting
                {
                    SettingID = 16,
                    SettingKey = "EnableSMSAlerts",
                    SettingValue = "true",
                    Description = "Enable/disable SMS alerts globally",
                    SettingType = "Bool"
                }
            );
        }
    }
}
