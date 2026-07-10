using System;

namespace LOTONetMonitor.Domain.Entities
{
    /// <summary>
    /// Represents an application setting/configuration
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int SettingID { get; set; }

        /// <summary>
        /// Setting key (unique identifier)
        /// </summary>
        public string SettingKey { get; set; }

        /// <summary>
        /// Setting value
        /// </summary>
        public string SettingValue { get; set; }

        /// <summary>
        /// Description of what this setting controls
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Type of setting: String, Int, Bool
        /// </summary>
        public string SettingType { get; set; } = "String";

        /// <summary>
        /// When the setting was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
