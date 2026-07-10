using System;

namespace LOTONetMonitor.Domain.Entities
{
    /// <summary>
    /// Represents an alert event for a device
    /// </summary>
    public class AlertHistory
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int AlertID { get; set; }

        /// <summary>
        /// Foreign key to Device
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// Type of alert: StatusChange, Recovery, Critical
        /// </summary>
        public string AlertType { get; set; }

        /// <summary>
        /// Previous device status
        /// </summary>
        public string PreviousStatus { get; set; }

        /// <summary>
        /// Current device status
        /// </summary>
        public string CurrentStatus { get; set; }

        /// <summary>
        /// Alert message details
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Was an email sent for this alert
        /// </summary>
        public bool EmailSent { get; set; } = false;

        /// <summary>
        /// Was an SMS sent for this alert
        /// </summary>
        public bool SMSSent { get; set; } = false;

        /// <summary>
        /// Has this alert been acknowledged by an operator
        /// </summary>
        public bool Acknowledged { get; set; } = false;

        /// <summary>
        /// Notes about the alert (e.g., resolution steps)
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// When the alert was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the alert was acknowledged
        /// </summary>
        public DateTime? AcknowledgedDate { get; set; }

        /// <summary>
        /// Who acknowledged the alert (username)
        /// </summary>
        public string AcknowledgedBy { get; set; }

        // Navigation property
        public Device Device { get; set; }
    }
}
