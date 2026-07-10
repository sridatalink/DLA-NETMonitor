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
    /// Authentication service implementation
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public async Task<(bool Success, string Message)> RegisterAsync(string email, string password, string fullName, string role = "Operator")
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                    return (false, "Email already registered");

                // Create new user
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    IsActive = true
                };

                // Create user with password
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Registration failed: {errors}");
                }

                // Ensure role exists
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                // Add user to role
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return (false, $"Failed to assign role: {errors}");
                }

                return (true, "Registration successful. Please log in.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Login a user
        /// </summary>
        public async Task<(bool Success, ApplicationUser User, string Message)> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return (false, null, "Invalid email or password");

                if (!user.IsActive)
                    return (false, null, "User account is disabled");

                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);

                if (result.Succeeded)
                    return (true, user, "Login successful");
                else if (result.IsLockedOut)
                    return (false, null, "Account locked due to too many failed login attempts");
                else if (result.RequiresTwoFactor)
                    return (false, null, "Two-factor authentication required");
                else
                    return (false, null, "Invalid email or password");
            }
            catch (Exception ex)
            {
                return (false, null, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Verify user password
        /// </summary>
        public async Task<bool> VerifyPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        public async Task<(bool Success, string Message)> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            try
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                    return (true, "Password changed successfully");

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Password change failed: {errors}");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Reset user password
        /// </summary>
        public async Task<(bool Success, string Message)> ResetPasswordAsync(ApplicationUser user, string newPassword)
        {
            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                    return (true, "Password reset successfully");

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Password reset failed: {errors}");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// Check if user is in role
        /// </summary>
        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        /// <summary>
        /// Add user to role
        /// </summary>
        public async Task<bool> AddUserToRoleAsync(ApplicationUser user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        /// <summary>
        /// Remove user from role
        /// </summary>
        public async Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }
    }
}
