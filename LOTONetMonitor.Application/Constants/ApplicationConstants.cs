namespace LOTONetMonitor.Application.Constants
{
    /// <summary>
    /// Application-wide constants
    /// </summary>
    public static class ApplicationConstants
    {
        // Roles
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_OPERATOR = "Operator";

        // Claim types
        public const string CLAIM_FULL_NAME = "FullName";
        public const string CLAIM_MOBILE = "MobileNumber";

        // Messages
        public const string OPERATION_SUCCESS = "Operation completed successfully";
        public const string OPERATION_FAILED = "Operation failed";
        public const string UNAUTHORIZED = "You are not authorized to perform this action";
        public const string NOT_FOUND = "Resource not found";
    }
}
