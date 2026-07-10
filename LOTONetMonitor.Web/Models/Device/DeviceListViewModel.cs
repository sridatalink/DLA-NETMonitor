using System.ComponentModel.DataAnnotations;

namespace LOTONetMonitor.Web.Models.Device
{
    /// <summary>
    /// View model for device list display
    /// </summary>
    public class DeviceListViewModel
    {
        public int DeviceID { get; set; }

        [Display(Name = "Device Name")]
        public string DeviceName { get; set; }

        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Status")]
        public string CurrentStatus { get; set; }

        [Display(Name = "Last Ping")]
        public DateTime? LastPing { get; set; }

        [Display(Name = "Response Time (ms)")]
        public int? ResponseTime { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Email Alert")]
        public bool EmailAlertEnabled { get; set; }

        [Display(Name = "SMS Alert")]
        public bool SMSAlertEnabled { get; set; }
    }
}
