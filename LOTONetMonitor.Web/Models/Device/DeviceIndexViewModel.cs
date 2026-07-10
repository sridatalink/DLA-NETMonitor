using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LOTONetMonitor.Web.Models.Device
{
    /// <summary>
    /// Container view model for device list with categories for filtering
    /// </summary>
    public class DeviceIndexViewModel
    {
        [Display(Name = "Filter by Category")]
        public int? SelectedCategoryID { get; set; }

        [Display(Name = "Filter by Status")]
        public string SelectedStatus { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public IEnumerable<DeviceListViewModel> Devices { get; set; } = new List<DeviceListViewModel>();
        public IEnumerable<CategoryDropdownViewModel> Categories { get; set; } = new List<CategoryDropdownViewModel>();
    }

    /// <summary>
    /// Category dropdown view model
    /// </summary>
    public class CategoryDropdownViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
