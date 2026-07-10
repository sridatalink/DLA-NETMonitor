using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOTONetMonitor.Application.Services.Interfaces;
using LOTONetMonitor.Domain.Constants;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Domain.Interfaces;
using LOTONetMonitor.Persistence.Repositories;

namespace LOTONetMonitor.Application.Services
{
    /// <summary>
    /// Device management service implementation
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(IUnitOfWork unitOfWork, ILogger<DeviceService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all active devices
        /// </summary>
        public async Task<IEnumerable<Device>> GetAllActiveDevicesAsync()
        {
            try
            {
                return await _unitOfWork.Devices.FindAsync(d => d.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving devices: {ex.Message}");
                return Enumerable.Empty<Device>();
            }
        }

        /// <summary>
        /// Get device by ID with details
        /// </summary>
        public async Task<Device> GetDeviceByIdAsync(int deviceID)
        {
            try
            {
                return await _unitOfWork.Devices.GetByIdAsync(deviceID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving device {deviceID}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        public async Task<(bool Success, string Message, int? DeviceID)> CreateDeviceAsync(Device device)
        {
            try
            {
                // Validate device
                if (device == null)
                    return (false, "Device cannot be null", null);

                if (string.IsNullOrWhiteSpace(device.DeviceName))
                    return (false, "Device name is required", null);

                if (string.IsNullOrWhiteSpace(device.IPAddress))
                    return (false, "IP address is required", null);

                // Check if IP already exists
                var ipExists = await _unitOfWork.Devices.ExistsAsync(d => d.IPAddress == device.IPAddress);
                if (ipExists)
                    return (false, "IP address already exists", null);

                // Check if category exists
                var categoryExists = await _unitOfWork.Categories.ExistsAsync(c => c.CategoryID == device.CategoryID);
                if (!categoryExists)
                    return (false, "Invalid category selected", null);

                // Set default values
                device.CreatedDate = DateTime.UtcNow;
                device.UpdatedDate = DateTime.UtcNow;
                device.CurrentStatus = StatusConstants.DEVICE_UNKNOWN;
                device.FailureCount = 0;

                // Add device
                await _unitOfWork.Devices.AddAsync(device);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Device '{device.DeviceName}' created successfully with ID {device.DeviceID}");
                return (true, "Device created successfully", device.DeviceID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating device: {ex.Message}");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Update an existing device
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateDeviceAsync(Device device)
        {
            try
            {
                if (device == null)
                    return (false, "Device cannot be null");

                var existingDevice = await _unitOfWork.Devices.GetByIdAsync(device.DeviceID);
                if (existingDevice == null)
                    return (false, "Device not found");

                // Check if new IP is unique (excluding current device)
                if (existingDevice.IPAddress != device.IPAddress)
                {
                    var ipExists = await _unitOfWork.Devices.ExistsAsync(d => d.IPAddress == device.IPAddress && d.DeviceID != device.DeviceID);
                    if (ipExists)
                        return (false, "IP address already exists");
                }

                // Check if category exists
                var categoryExists = await _unitOfWork.Categories.ExistsAsync(c => c.CategoryID == device.CategoryID);
                if (!categoryExists)
                    return (false, "Invalid category selected");

                // Update properties
                existingDevice.DeviceName = device.DeviceName;
                existingDevice.IPAddress = device.IPAddress;
                existingDevice.CategoryID = device.CategoryID;
                existingDevice.Location = device.Location;
                existingDevice.Description = device.Description;
                existingDevice.PingIntervalSeconds = device.PingIntervalSeconds;
                existingDevice.TimeoutThreshold = device.TimeoutThreshold;
                existingDevice.EmailAlertEnabled = device.EmailAlertEnabled;
                existingDevice.SMSAlertEnabled = device.SMSAlertEnabled;
                existingDevice.IsActive = device.IsActive;
                existingDevice.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Devices.UpdateAsync(existingDevice);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Device '{existingDevice.DeviceName}' updated successfully");
                return (true, "Device updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating device: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a device (soft delete by deactivating)
        /// </summary>
        public async Task<(bool Success, string Message)> DeleteDeviceAsync(int deviceID)
        {
            try
            {
                var device = await _unitOfWork.Devices.GetByIdAsync(deviceID);
                if (device == null)
                    return (false, "Device not found");

                device.IsActive = false;
                device.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Devices.UpdateAsync(device);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Device '{device.DeviceName}' deleted successfully");
                return (true, "Device deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting device: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get devices by category
        /// </summary>
        public async Task<IEnumerable<Device>> GetDevicesByCategoryAsync(int categoryID)
        {
            try
            {
                return await _unitOfWork.Devices.FindAsync(d => d.CategoryID == categoryID && d.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving devices by category: {ex.Message}");
                return Enumerable.Empty<Device>();
            }
        }

        /// <summary>
        /// Get devices by status
        /// </summary>
        public async Task<IEnumerable<Device>> GetDevicesByStatusAsync(string status)
        {
            try
            {
                return await _unitOfWork.Devices.FindAsync(d => d.CurrentStatus == status && d.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving devices by status: {ex.Message}");
                return Enumerable.Empty<Device>();
            }
        }

        /// <summary>
        /// Search devices by name or IP
        /// </summary>
        public async Task<IEnumerable<Device>> SearchDevicesAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllActiveDevicesAsync();

                return await _unitOfWork.Devices.FindAsync(d =>
                    d.IsActive &&
                    (d.DeviceName.Contains(searchTerm) || d.IPAddress.Contains(searchTerm))
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching devices: {ex.Message}");
                return Enumerable.Empty<Device>();
            }
        }

        /// <summary>
        /// Get device count statistics
        /// </summary>
        public async Task<(int Total, int Online, int Offline, int Warning)> GetDeviceStatisticsAsync()
        {
            try
            {
                var devices = await GetAllActiveDevicesAsync();
                var deviceList = devices.ToList();

                var total = deviceList.Count;
                var online = deviceList.Count(d => d.CurrentStatus == StatusConstants.DEVICE_ONLINE);
                var offline = deviceList.Count(d => d.CurrentStatus == StatusConstants.DEVICE_OFFLINE);
                var warning = deviceList.Count(d => d.CurrentStatus == StatusConstants.DEVICE_WARNING);

                return (total, online, offline, warning);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating device statistics: {ex.Message}");
                return (0, 0, 0, 0);
            }
        }
    }
}
