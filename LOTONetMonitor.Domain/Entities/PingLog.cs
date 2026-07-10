using System;

namespace LOTONetMonitor.Domain.Entities
{
    /// <summary>
    /// Represents a single ping attempt to a device
    /// </summary>
    public class PingLog
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// Foreign key to Device
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// When the ping was sent
        /// </summary>
        public DateTime PingTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Response time in milliseconds (null if no response)
        /// </summary>
        public int? ResponseTime { get; set; }

        /// <summary>
        /// Status of the ping: Online, Offline, Timeout
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Record creation date
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Device Device { get; set; }
    }
}
