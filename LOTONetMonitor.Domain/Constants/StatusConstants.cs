namespace LOTONetMonitor.Domain.Constants
{
    /// <summary>
    /// Constants for device and alert statuses
    /// </summary>
    public static class StatusConstants
    {
        // Device Status
        public const string DEVICE_ONLINE = "Online";
        public const string DEVICE_OFFLINE = "Offline";
        public const string DEVICE_WARNING = "Warning";
        public const string DEVICE_UNKNOWN = "Unknown";

        // Alert Types
        public const string ALERT_STATUS_CHANGE = "StatusChange";
        public const string ALERT_RECOVERY = "Recovery";
        public const string ALERT_CRITICAL = "Critical";

        // Ping Status
        public const string PING_ONLINE = "Online";
        public const string PING_OFFLINE = "Offline";
        public const string PING_TIMEOUT = "Timeout";

        // Setting Types
        public const string SETTING_TYPE_STRING = "String";
        public const string SETTING_TYPE_INT = "Int";
        public const string SETTING_TYPE_BOOL = "Bool";
    }
}
