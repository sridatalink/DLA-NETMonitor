using System.Collections.Generic;
using System.Threading.Tasks;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Application.Services.Interfaces
{
    /// <summary>
    /// Interface for user management services
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get all active users
        /// </summary>
        Task<IEnumerable<ApplicationUser>> GetAllActiveUsersAsync();

        /// <summary>
        /// Get user with details by ID
        /// </summary>
        Task<ApplicationUser> GetUserWithDetailsAsync(string userId);

        /// <summary>
        /// Update user profile
        /// </summary>
        Task<(bool Success, string Message)> UpdateUserProfileAsync(ApplicationUser user);

        /// <summary>
        /// Deactivate/Disable user account
        /// </summary>
        Task<(bool Success, string Message)> DeactivateUserAsync(string userId);

        /// <summary>
        /// Activate user account
        /// </summary>
        Task<(bool Success, string Message)> ActivateUserAsync(string userId);

        /// <summary>
        /// Get user roles
        /// </summary>
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);

        /// <summary>
        /// Update user roles
        /// </summary>
        Task<(bool Success, string Message)> UpdateUserRolesAsync(ApplicationUser user, IList<string> roles);
    }
}
