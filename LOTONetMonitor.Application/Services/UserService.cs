using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LOTONetMonitor.Application.Services.Interfaces;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Application.Services
{
    /// <summary>
    /// User management service implementation
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        /// <summary>
        /// Get all active users
        /// </summary>
        public async Task<IEnumerable<ApplicationUser>> GetAllActiveUsersAsync()
        {
            return await _userManager.Users.Where(u => u.IsActive).ToListAsync();
        }

        /// <summary>
        /// Get user with details by ID
        /// </summary>
        public async Task<ApplicationUser> GetUserWithDetailsAsync(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateUserProfileAsync(ApplicationUser user)
        {
            try
            {
                user.UpdatedDate = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return (true, "Profile updated successfully");

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Profile update failed: {errors}");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deactivate user account
        /// </summary>
        public async Task<(bool Success, string Message)> DeactivateUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return (false, "User not found");

                user.IsActive = false;
                user.UpdatedDate = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return (true, "User deactivated successfully");

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Deactivation failed: {errors}");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Activate user account
        /// </summary>
        public async Task<(bool Success, string Message)> ActivateUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return (false, "User not found");

                user.IsActive = true;
                user.UpdatedDate = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return (true, "User activated successfully");

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Activation failed: {errors}");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get user roles
        /// </summary>
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Update user roles
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateUserRolesAsync(ApplicationUser user, IList<string> roles)
        {
            try
            {
                // Get current roles
                var userRoles = await _userManager.GetRolesAsync(user);

                // Remove roles not in new list
                var rolesToRemove = userRoles.Except(roles).ToList();
                if (rolesToRemove.Count > 0)
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                        return (false, "Failed to remove some roles");
                }

                // Add new roles
                var rolesToAdd = roles.Except(userRoles).ToList();
                if (rolesToAdd.Count > 0)
                {
                    // Ensure roles exist
                    foreach (var role in rolesToAdd)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                            await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addResult.Succeeded)
                        return (false, "Failed to add some roles");
                }

                return (true, "Roles updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }
    }
}
