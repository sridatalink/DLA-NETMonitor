using Microsoft.AspNetCore.Identity;

namespace LOTONetMonitor.Domain.Entities
{
    /// <summary>
    /// Extended ASP.NET Identity User for the application
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Mobile number for SMS alerts
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Is this user account active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
