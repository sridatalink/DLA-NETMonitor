using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LOTONetMonitor.Application.Constants;
using LOTONetMonitor.Application.Services.Interfaces;
using LOTONetMonitor.Web.Models.Auth;

namespace LOTONetMonitor.Web.Controllers
{
    /// <summary>
    /// Handle authentication and authorization operations
    /// </summary>
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Display login page
        /// </summary>
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Handle login form submission
        /// </summary>
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var (success, user, message) = await _authService.LoginAsync(model.Email, model.Password);

                if (success)
                {
                    _logger.LogInformation($"User '{model.Email}' logged in successfully");

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError(string.Empty, message);
                _logger.LogWarning($"Failed login attempt for user '{model.Email}': {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred during login");
            }

            return View(model);
        }

        /// <summary>
        /// Display registration page
        /// </summary>
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handle registration form submission
        /// </summary>
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var (success, message) = await _authService.RegisterAsync(
                    model.Email,
                    model.Password,
                    model.FullName,
                    ApplicationConstants.ROLE_OPERATOR
                );

                if (success)
                {
                    _logger.LogInformation($"New user '{model.Email}' registered successfully");
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, message);
                _logger.LogWarning($"Registration failed for '{model.Email}': {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registration: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred during registration");
            }

            return View(model);
        }

        /// <summary>
        /// Handle logout
        /// </summary>
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = User.Identity?.Name;
                await _authService.LogoutAsync();
                _logger.LogInformation($"User '{user}' logged out");
                TempData["InfoMessage"] = "You have been logged out successfully";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during logout: {ex.Message}");
                return RedirectToAction("Index", "Dashboard");
            }
        }
    }
}
