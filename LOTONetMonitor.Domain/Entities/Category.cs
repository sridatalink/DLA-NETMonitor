using System;
using System.Collections.Generic;

namespace LOTONetMonitor.Domain.Entities
{
    /// <summary>
    /// Represents a device category (e.g., Data Center, Servers, Routers)
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Category name (e.g., "Data Center", "Servers")
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Category description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// When the category was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the category was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Is this category active
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
