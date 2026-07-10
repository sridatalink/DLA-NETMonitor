using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Domain.Interfaces;

namespace LOTONetMonitor.Persistence.Repositories
{
    /// <summary>
    /// Device-specific repository with advanced queries
    /// </summary>
    public class DeviceRepository : Repository<Device>
    {
        public DeviceRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all active devices with category information
        /// </summary>
        public async Task<IEnumerable<Device>> GetAllActiveDevicesWithCategoryAsync()
        {
            return await _dbSet
                .Where(d => d.IsActive)
                .Include(d => d.Category)
                .OrderBy(d => d.DeviceName)
                .ToListAsync();
        }

        /// <summary>
        /// Get devices by category
        /// </summary>
        public async Task<IEnumerable<Device>> GetDevicesByCategoryAsync(int categoryID)
        {
            return await _dbSet
                .Where(d => d.CategoryID == categoryID && d.IsActive)
                .Include(d => d.Category)
                .OrderBy(d => d.DeviceName)
                .ToListAsync();
        }

        /// <summary>
        /// Get devices by status
        /// </summary>
        public async Task<IEnumerable<Device>> GetDevicesByStatusAsync(string status)
        {
            return await _dbSet
                .Where(d => d.CurrentStatus == status && d.IsActive)
                .Include(d => d.Category)
                .OrderBy(d => d.DeviceName)
                .ToListAsync();
        }

        /// <summary>
        /// Get device with all related data (category, last pings, alerts)
        /// </summary>
        public async Task<Device> GetDeviceWithDetailsAsync(int deviceID)
        {
            return await _dbSet
                .Include(d => d.Category)
                .Include(d => d.PingLogs.OrderByDescending(p => p.PingTime).Take(10))
                .Include(d => d.AlertHistories.OrderByDescending(a => a.CreatedDate).Take(5))
                .FirstOrDefaultAsync(d => d.DeviceID == deviceID && d.IsActive);
        }

        /// <summary>
        /// Get all devices for monitoring (including inactive ones)
        /// </summary>
        public async Task<IEnumerable<Device>> GetAllDevicesForMonitoringAsync()
        {
            return await _dbSet
                .Where(d => d.IsActive)
                .Include(d => d.Category)
                .ToListAsync();
        }

        /// <summary>
        /// Check if IP address already exists (excluding current device)
        /// </summary>
        public async Task<bool> IsIPAddressUniqueAsync(string ipAddress, int excludeDeviceID = 0)
        {
            return !await _dbSet.AnyAsync(d => d.IPAddress == ipAddress && d.DeviceID != excludeDeviceID);
        }

        /// <summary>
        /// Get devices by search term (name or IP)
        /// </summary>
        public async Task<IEnumerable<Device>> SearchDevicesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllActiveDevicesWithCategoryAsync();

            return await _dbSet
                .Where(d => d.IsActive && (d.DeviceName.Contains(searchTerm) || d.IPAddress.Contains(searchTerm)))
                .Include(d => d.Category)
                .OrderBy(d => d.DeviceName)
                .ToListAsync();
        }

        /// <summary>
        /// Get offline devices (with failure count >= threshold)
        /// </summary>
        public async Task<IEnumerable<Device>> GetOfflineDevicesAsync()
        {
            return await _dbSet
                .Where(d => d.CurrentStatus == "Offline" && d.IsActive)
                .Include(d => d.Category)
                .ToListAsync();
        }

        /// <summary>
        /// Get warning devices (high response time or recent failures)
        /// </summary>
        public async Task<IEnumerable<Device>> GetWarningDevicesAsync()
        {
            return await _dbSet
                .Where(d => d.CurrentStatus == "Warning" && d.IsActive)
                .Include(d => d.Category)
                .ToListAsync();
        }
    }
}
