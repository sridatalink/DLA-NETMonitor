using System.Collections.Generic;
using System.Threading.Tasks;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Application.Services.Interfaces
{
    /// <summary>
    /// Interface for category management services
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Get all active categories
        /// </summary>
        Task<IEnumerable<Category>> GetAllActiveCategoriesAsync();

        /// <summary>
        /// Get category by ID
        /// </summary>
        Task<Category> GetCategoryByIdAsync(int categoryID);

        /// <summary>
        /// Create a new category
        /// </summary>
        Task<(bool Success, string Message)> CreateCategoryAsync(Category category);

        /// <summary>
        /// Update an existing category
        /// </summary>
        Task<(bool Success, string Message)> UpdateCategoryAsync(Category category);

        /// <summary>
        /// Delete a category
        /// </summary>
        Task<(bool Success, string Message)> DeleteCategoryAsync(int categoryID);

        /// <summary>
        /// Get all categories with device count
        /// </summary>
        Task<IEnumerable<(Category Category, int DeviceCount)>> GetCategoriesWithCountAsync();
    }
}
