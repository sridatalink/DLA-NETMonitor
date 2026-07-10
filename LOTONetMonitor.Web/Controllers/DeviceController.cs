using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LOTONetMonitor.Application.Services.Interfaces;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Web.Models.Device;

namespace LOTONetMonitor.Web.Controllers
{
    /// <summary>
    /// Controller for device management operations
    /// </summary>
    [Authorize]
    [Route("devices")]
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(
            IDeviceService deviceService,
            ICategoryService categoryService,
            ILogger<DeviceController> logger)
        {
            _deviceService = deviceService ?? throw new ArgumentNullException(nameof(deviceService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Display list of all devices
        /// </summary>
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(int? categoryID, string status, string searchTerm)
        {
            try
            {
                IEnumerable<Device> devices;

                // Apply filters
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    devices = await _deviceService.SearchDevicesAsync(searchTerm);
                }
                else if (categoryID.HasValue)
                {
                    devices = await _deviceService.GetDevicesByCategoryAsync(categoryID.Value);
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    devices = await _deviceService.GetDevicesByStatusAsync(status);
                }
                else
                {
                    devices = await _deviceService.GetAllActiveDevicesAsync();
                }

                var categories = await _categoryService.GetAllActiveCategoriesAsync();

                var viewModel = new DeviceIndexViewModel
                {
                    SelectedCategoryID = categoryID,
                    SelectedStatus = status,
                    SearchTerm = searchTerm,
                    Devices = devices.Select(d => new DeviceListViewModel
                    {
                        DeviceID = d.DeviceID,
                        DeviceName = d.DeviceName,
                        IPAddress = d.IPAddress,
                        CategoryName = d.Category?.CategoryName ?? "N/A",
                        Location = d.Location,
                        CurrentStatus = d.CurrentStatus,
                        LastPing = d.LastPing,
                        ResponseTime = d.ResponseTime,
                        IsActive = d.IsActive,
                        EmailAlertEnabled = d.EmailAlertEnabled,
                        SMSAlertEnabled = d.SMSAlertEnabled
                    }).ToList(),
                    Categories = categories.Select(c => new CategoryDropdownViewModel
                    {
                        CategoryID = c.CategoryID,
                        CategoryName = c.CategoryName
                    }).ToList()
                };

                _logger.LogInformation($"Device list displayed: {viewModel.Devices.Count()} devices");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading device list: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading devices";
                return RedirectToAction("Index", "Dashboard");
            }
        }

        /// <summary>
        /// Display device creation form
        /// </summary>
        [HttpGet("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryService.GetAllActiveCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading create form: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading the form";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Handle device creation
        /// </summary>
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DeviceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllActiveCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName", model.CategoryID);
                return View(model);
            }

            try
            {
                var device = new Device
                {
                    DeviceName = model.DeviceName,
                    IPAddress = model.IPAddress,
                    CategoryID = model.CategoryID,
                    Location = model.Location,
                    Description = model.Description,
                    PingIntervalSeconds = model.PingIntervalSeconds,
                    TimeoutThreshold = model.TimeoutThreshold,
                    EmailAlertEnabled = model.EmailAlertEnabled,
                    SMSAlertEnabled = model.SMSAlertEnabled,
                    IsActive = model.IsActive
                };

                var (success, message, deviceID) = await _deviceService.CreateDeviceAsync(device);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, message);
                _logger.LogWarning($"Device creation failed: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating device: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the device");
            }

            var categoriesForForm = await _categoryService.GetAllActiveCategoriesAsync();
            ViewBag.Categories = new SelectList(categoriesForForm, "CategoryID", "CategoryName", model.CategoryID);
            return View(model);
        }

        /// <summary>
        /// Display device edit form
        /// </summary>
        [HttpGet("{id}/edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var device = await _deviceService.GetDeviceByIdAsync(id);
                if (device == null)
                {
                    TempData["ErrorMessage"] = "Device not found";
                    return RedirectToAction("Index");
                }

                var model = new DeviceViewModel
                {
                    DeviceID = device.DeviceID,
                    DeviceName = device.DeviceName,
                    IPAddress = device.IPAddress,
                    CategoryID = device.CategoryID,
                    Location = device.Location,
                    Description = device.Description,
                    PingIntervalSeconds = device.PingIntervalSeconds,
                    TimeoutThreshold = device.TimeoutThreshold,
                    EmailAlertEnabled = device.EmailAlertEnabled,
                    SMSAlertEnabled = device.SMSAlertEnabled,
                    IsActive = device.IsActive,
                    CurrentStatus = device.CurrentStatus,
                    LastPing = device.LastPing,
                    ResponseTime = device.ResponseTime,
                    FailureCount = device.FailureCount
                };

                var categories = await _categoryService.GetAllActiveCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName", model.CategoryID);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading edit form: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading the device";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Handle device update
        /// </summary>
        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, DeviceViewModel model)
        {
            if (id != model.DeviceID)
            {
                TempData["ErrorMessage"] = "Invalid device ID";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllActiveCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName", model.CategoryID);
                return View(model);
            }

            try
            {
                var device = new Device
                {
                    DeviceID = model.DeviceID,
                    DeviceName = model.DeviceName,
                    IPAddress = model.IPAddress,
                    CategoryID = model.CategoryID,
                    Location = model.Location,
                    Description = model.Description,
                    PingIntervalSeconds = model.PingIntervalSeconds,
                    TimeoutThreshold = model.TimeoutThreshold,
                    EmailAlertEnabled = model.EmailAlertEnabled,
                    SMSAlertEnabled = model.SMSAlertEnabled,
                    IsActive = model.IsActive
                };

                var (success, message) = await _deviceService.UpdateDeviceAsync(device);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, message);
                _logger.LogWarning($"Device update failed: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating device: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the device");
            }

            var categoriesForForm = await _categoryService.GetAllActiveCategoriesAsync();
            ViewBag.Categories = new SelectList(categoriesForForm, "CategoryID", "CategoryName", model.CategoryID);
            return View(model);
        }

        /// <summary>
        /// Display device deletion confirmation
        /// </summary>
        [HttpGet("{id}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var device = await _deviceService.GetDeviceByIdAsync(id);
                if (device == null)
                {
                    TempData["ErrorMessage"] = "Device not found";
                    return RedirectToAction("Index");
                }

                var model = new DeviceListViewModel
                {
                    DeviceID = device.DeviceID,
                    DeviceName = device.DeviceName,
                    IPAddress = device.IPAddress,
                    CategoryName = device.Category?.CategoryName ?? "N/A",
                    Location = device.Location,
                    CurrentStatus = device.CurrentStatus
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading delete confirmation: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Handle device deletion
        /// </summary>
        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var (success, message) = await _deviceService.DeleteDeviceAsync(id);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = message;
                return RedirectToAction("Delete", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting device: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the device";
                return RedirectToAction("Index");
            }
        }
    }
}
