using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Domain.Interfaces;

namespace LOTONetMonitor.Application.Services.Interfaces
{
    /// <summary>
    /// Interface for device management services
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// Get all active devices
        /// </summary>
        Task<IEnumerable<Device>> GetAllActiveDevicesAsync();

        /// <summary>
        /// Get device by ID with details
        /// </summary>
        Task<Device> GetDeviceByIdAsync(int deviceID);

        /// <summary>
        /// Create a new device
        /// </summary>
        Task<(bool Success, string Message, int? DeviceID)> CreateDeviceAsync(Device device);

        /// <summary>
        /// Update an existing device
        /// </summary>
        Task<(bool Success, string Message)> UpdateDeviceAsync(Device device);

        /// <summary>
        /// Delete a device
        /// </summary>
        Task<(bool Success, string Message)> DeleteDeviceAsync(int deviceID);

        /// <summary>
        /// Get devices by category
        /// </summary>
        Task<IEnumerable<Device>> GetDevicesByCategoryAsync(int categoryID);

        /// <summary>
        /// Get devices by status
        /// </summary>
        Task<IEnumerable<Device>> GetDevicesByStatusAsync(string status);

        /// <summary>
        /// Search devices by name or IP
        /// </summary>
        Task<IEnumerable<Device>> SearchDevicesAsync(string searchTerm);

        /// <summary>
        /// Get device count statistics
        /// </summary>
        Task<(int Total, int Online, int Offline, int Warning)> GetDeviceStatisticsAsync();
    }
}
