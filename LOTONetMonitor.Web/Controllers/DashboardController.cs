using Microsoft.AspNetCore.Mvc;

namespace LOTONetMonitor.Web.Controllers
{
    /// <summary>
    /// Dashboard controller for displaying monitoring status
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Display the main dashboard
        /// </summary>
        [HttpGet("/")]
        [HttpGet("dashboard")]
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Dashboard accessed by user: {User}", User.Identity?.Name);
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading dashboard: {ex.Message}");
                return StatusCode(500, "An error occurred while loading the dashboard");
            }
        }
    }
}
