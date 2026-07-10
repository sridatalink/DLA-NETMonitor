using System;
using System.Collections.Generic;

namespace LOTONetMonitor.Domain.Entities
{
    /// <summary>
    /// Represents a network device to be monitored
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// Foreign key to Category
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Name of the device
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// IP address of the device (IPv4 or IPv6)
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Physical or logical location of the device
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Device description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Enable email alerts for this device
        /// </summary>
        public bool EmailAlertEnabled { get; set; } = true;

        /// <summary>
        /// Enable SMS alerts for this device
        /// </summary>
        public bool SMSAlertEnabled { get; set; } = true;

        /// <summary>
        /// Interval in seconds between pings (default: 30 seconds)
        /// </summary>
        public int PingIntervalSeconds { get; set; } = 30;

        /// <summary>
        /// Number of consecutive failures before marking as Offline (default: 3)
        /// </summary>
        public int TimeoutThreshold { get; set; } = 3;

        /// <summary>
        /// Current status: Online, Offline, Warning, Unknown
        /// </summary>
        public string CurrentStatus { get; set; } = "Unknown";

        /// <summary>
        /// Last successful ping time
        /// </summary>
        public DateTime? LastPing { get; set; }

        /// <summary>
        /// Response time in milliseconds from last ping
        /// </summary>
        public int? ResponseTime { get; set; }

        /// <summary>
        /// Current failure count (reset to 0 on success)
        /// </summary>
        public int FailureCount { get; set; } = 0;

        /// <summary>
        /// When the device was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the device was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Is this device active and being monitored
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Category Category { get; set; }
        public ICollection<PingLog> PingLogs { get; set; } = new List<PingLog>();
        public ICollection<AlertHistory> AlertHistories { get; set; } = new List<AlertHistory>();
    }
}
