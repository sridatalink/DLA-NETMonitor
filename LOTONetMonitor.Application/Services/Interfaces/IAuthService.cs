using System.Threading.Tasks;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Application.Services.Interfaces
{
    /// <summary>
    /// Interface for authentication and identity services
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Register a new user
        /// </summary>
        Task<(bool Success, string Message)> RegisterAsync(string email, string password, string fullName, string role = "Operator");

        /// <summary>
        /// Login a user
        /// </summary>
        Task<(bool Success, ApplicationUser User, string Message)> LoginAsync(string email, string password);

        /// <summary>
        /// Logout current user
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Verify user password
        /// </summary>
        Task<bool> VerifyPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Change user password
        /// </summary>
        Task<(bool Success, string Message)> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        /// <summary>
        /// Reset user password
        /// </summary>
        Task<(bool Success, string Message)> ResetPasswordAsync(ApplicationUser user, string newPassword);

        /// <summary>
        /// Get user by email
        /// </summary>
        Task<ApplicationUser> GetUserByEmailAsync(string email);

        /// <summary>
        /// Get user by ID
        /// </summary>
        Task<ApplicationUser> GetUserByIdAsync(string userId);

        /// <summary>
        /// Check if user is in role
        /// </summary>
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string role);

        /// <summary>
        /// Add user to role
        /// </summary>
        Task<bool> AddUserToRoleAsync(ApplicationUser user, string role);

        /// <summary>
        /// Remove user from role
        /// </summary>
        Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string role);
    }
}
