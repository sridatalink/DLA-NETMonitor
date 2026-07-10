using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LOTONetMonitor.Web.Models.Category
{
    /// <summary>
    /// View model for category list display
    /// </summary>
    public class CategoryListViewModel
    {
        public int CategoryID { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Device Count")]
        public int DeviceCount { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; }
    }
}
