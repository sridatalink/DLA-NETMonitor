using System;
using System.Threading.Tasks;
using LOTONetMonitor.Domain.Entities;

namespace LOTONetMonitor.Domain.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface for managing repositories and transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> Categories { get; }
        IRepository<Device> Devices { get; }
        IRepository<PingLog> PingLogs { get; }
        IRepository<AlertHistory> AlertHistories { get; }
        IRepository<Setting> Settings { get; }

        /// <summary>
        /// Save all changes to database
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begin a database transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        Task RollbackAsync();
    }
}
