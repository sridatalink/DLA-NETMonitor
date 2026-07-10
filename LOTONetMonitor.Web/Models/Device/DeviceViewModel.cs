using System.ComponentModel.DataAnnotations;

namespace LOTONetMonitor.Web.Models.Device
{
    /// <summary>
    /// View model for device creation and editing
    /// </summary>
    public class DeviceViewModel
    {
        public int DeviceID { get; set; }

        [Required(ErrorMessage = "Device name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Device name must be between 2 and 200 characters")]
        [Display(Name = "Device Name")]
        public string DeviceName { get; set; }

        [Required(ErrorMessage = "IP Address is required")]
        [RegularExpression(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$|^([0-9a-fA-F]{0,4}:){2,7}[0-9a-fA-F]{0,4}$",
            ErrorMessage = "Please enter a valid IPv4 or IPv6 address")]
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [StringLength(300, ErrorMessage = "Location cannot exceed 300 characters")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Range(10, 3600, ErrorMessage = "Ping interval must be between 10 and 3600 seconds")]
        [Display(Name = "Ping Interval (seconds)")]
        public int PingIntervalSeconds { get; set; } = 30;

        [Range(1, 10, ErrorMessage = "Timeout threshold must be between 1 and 10")]
        [Display(Name = "Timeout Threshold")]
        public int TimeoutThreshold { get; set; } = 3;

        [Display(Name = "Email Alert Enabled")]
        public bool EmailAlertEnabled { get; set; } = true;

        [Display(Name = "SMS Alert Enabled")]
        public bool SMSAlertEnabled { get; set; } = true;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Display only properties
        [Display(Name = "Current Status")]
        public string CurrentStatus { get; set; } = "Unknown";

        [Display(Name = "Last Ping")]
        public DateTime? LastPing { get; set; }

        [Display(Name = "Response Time (ms)")]
        public int? ResponseTime { get; set; }

        [Display(Name = "Failure Count")]
        public int FailureCount { get; set; } = 0;
    }
}
