using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LOTONetMonitor.Domain.Interfaces
{
    /// <summary>
    /// Generic repository interface for data access operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get entity by ID
        /// </summary>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Get entity by ID (long version for entities with long IDs)
        /// </summary>
        Task<T> GetByIdAsync(long id);

        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get entities matching a predicate
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add a new entity
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Add multiple entities
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update an entity
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Delete an entity
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Delete multiple entities
        /// </summary>
        Task DeleteRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Check if entity exists matching predicate
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get count of entities
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// Get count of entities matching predicate
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
