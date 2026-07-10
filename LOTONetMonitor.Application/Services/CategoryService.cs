using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOTONetMonitor.Application.Services.Interfaces;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Domain.Interfaces;

namespace LOTONetMonitor.Application.Services
{
    /// <summary>
    /// Category management service implementation
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all active categories
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllActiveCategoriesAsync()
        {
            try
            {
                return await _unitOfWork.Categories.FindAsync(c => c.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving categories: {ex.Message}");
                return Enumerable.Empty<Category>();
            }
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        public async Task<Category> GetCategoryByIdAsync(int categoryID)
        {
            try
            {
                return await _unitOfWork.Categories.GetByIdAsync(categoryID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving category {categoryID}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        public async Task<(bool Success, string Message)> CreateCategoryAsync(Category category)
        {
            try
            {
                if (category == null)
                    return (false, "Category cannot be null");

                if (string.IsNullOrWhiteSpace(category.CategoryName))
                    return (false, "Category name is required");

                // Check if category name already exists
                var exists = await _unitOfWork.Categories.ExistsAsync(c => c.CategoryName == category.CategoryName);
                if (exists)
                    return (false, "Category name already exists");

                category.CreatedDate = DateTime.UtcNow;
                category.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Category '{category.CategoryName}' created successfully");
                return (true, "Category created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateCategoryAsync(Category category)
        {
            try
            {
                if (category == null)
                    return (false, "Category cannot be null");

                var existingCategory = await _unitOfWork.Categories.GetByIdAsync(category.CategoryID);
                if (existingCategory == null)
                    return (false, "Category not found");

                // Check if new name is unique (excluding current category)
                if (existingCategory.CategoryName != category.CategoryName)
                {
                    var exists = await _unitOfWork.Categories.ExistsAsync(c => c.CategoryName == category.CategoryName && c.CategoryID != category.CategoryID);
                    if (exists)
                        return (false, "Category name already exists");
                }

                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Description = category.Description;
                existingCategory.IsActive = category.IsActive;
                existingCategory.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Categories.UpdateAsync(existingCategory);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Category '{existingCategory.CategoryName}' updated successfully");
                return (true, "Category updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a category (soft delete)
        /// </summary>
        public async Task<(bool Success, string Message)> DeleteCategoryAsync(int categoryID)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryID);
                if (category == null)
                    return (false, "Category not found");

                // Check if category has active devices
                var hasDevices = await _unitOfWork.Devices.ExistsAsync(d => d.CategoryID == categoryID && d.IsActive);
                if (hasDevices)
                    return (false, "Cannot delete category with active devices");

                category.IsActive = false;
                category.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Category '{category.CategoryName}' deleted successfully");
                return (true, "Category deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all categories with device count
        /// </summary>
        public async Task<IEnumerable<(Category Category, int DeviceCount)>> GetCategoriesWithCountAsync()
        {
            try
            {
                var categories = await GetAllActiveCategoriesAsync();
                var allDevices = await _unitOfWork.Devices.FindAsync(d => d.IsActive);

                return categories.Select(c => (c, allDevices.Count(d => d.CategoryID == c.CategoryID))).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving categories with count: {ex.Message}");
                return Enumerable.Empty<(Category, int)>();
            }
        }
    }
}
