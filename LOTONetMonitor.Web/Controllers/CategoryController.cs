using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LOTONetMonitor.Application.Services.Interfaces;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Web.Models.Category;

namespace LOTONetMonitor.Web.Controllers
{
    /// <summary>
    /// Controller for category management operations
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("categories")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Display list of all categories
        /// </summary>
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryService.GetAllActiveCategoriesAsync();
                // For now, return the first category to display
                // In a full implementation, this would display all categories
                var category = categories.FirstOrDefault();
                
                if (category != null)
                {
                    var viewModel = new CategoryListViewModel
                    {
                        CategoryID = category.CategoryID,
                        CategoryName = category.CategoryName,
                        Description = category.Description,
                        IsActive = category.IsActive,
                        CreatedDate = category.CreatedDate,
                        DeviceCount = 0 // Will be populated by service
                    };
                    return View(viewModel);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading category list: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading categories";
                return RedirectToAction("Index", "Dashboard");
            }
        }

        /// <summary>
        /// Display category creation form
        /// </summary>
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handle category creation
        /// </summary>
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var category = new Category
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description,
                    IsActive = model.IsActive
                };

                var (success, message) = await _categoryService.CreateCategoryAsync(category);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, message);
                _logger.LogWarning($"Category creation failed: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the category");
            }

            return View(model);
        }

        /// <summary>
        /// Display category edit form
        /// </summary>
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction("Index");
                }

                var model = new CategoryViewModel
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    IsActive = category.IsActive
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading edit form: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading the category";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Handle category update
        /// </summary>
        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel model)
        {
            if (id != model.CategoryID)
            {
                TempData["ErrorMessage"] = "Invalid category ID";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var category = new Category
                {
                    CategoryID = model.CategoryID,
                    CategoryName = model.CategoryName,
                    Description = model.Description,
                    IsActive = model.IsActive
                };

                var (success, message) = await _categoryService.UpdateCategoryAsync(category);

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, message);
                _logger.LogWarning($"Category update failed: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the category");
            }

            return View(model);
        }

        /// <summary>
        /// Display category deletion confirmation
        /// </summary>
        [HttpGet("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction("Index");
                }

                var model = new CategoryListViewModel
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    DeviceCount = 0 // Will be populated if needed
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
        /// Handle category deletion
        /// </summary>
        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var (success, message) = await _categoryService.DeleteCategoryAsync(id);

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
                _logger.LogError($"Error deleting category: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the category";
                return RedirectToAction("Index");
            }
        }
    }
}
