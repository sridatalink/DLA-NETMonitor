using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Domain.Interfaces;
using LOTONetMonitor.Persistence.Data;
using LOTONetMonitor.Persistence.Repositories;

namespace LOTONetMonitor.Persistence.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation managing all repositories and database transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        private IRepository<Category> _categoryRepository;
        private IRepository<Device> _deviceRepository;
        private IRepository<PingLog> _pingLogRepository;
        private IRepository<AlertHistory> _alertHistoryRepository;
        private IRepository<Setting> _settingRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Category repository
        /// </summary>
        public IRepository<Category> Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new Repository<Category>(_context);
                return _categoryRepository;
            }
        }

        /// <summary>
        /// Device repository
        /// </summary>
        public IRepository<Device> Devices
        {
            get
            {
                if (_deviceRepository == null)
                    _deviceRepository = new Repository<Device>(_context);
                return _deviceRepository;
            }
        }

        /// <summary>
        /// Ping log repository
        /// </summary>
        public IRepository<PingLog> PingLogs
        {
            get
            {
                if (_pingLogRepository == null)
                    _pingLogRepository = new Repository<PingLog>(_context);
                return _pingLogRepository;
            }
        }

        /// <summary>
        /// Alert history repository
        /// </summary>
        public IRepository<AlertHistory> AlertHistories
        {
            get
            {
                if (_alertHistoryRepository == null)
                    _alertHistoryRepository = new Repository<AlertHistory>(_context);
                return _alertHistoryRepository;
            }
        }

        /// <summary>
        /// Settings repository
        /// </summary>
        public IRepository<Setting> Settings
        {
            get
            {
                if (_settingRepository == null)
                    _settingRepository = new Repository<Setting>(_context);
                return _settingRepository;
            }
        }

        /// <summary>
        /// Save all changes to database
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Begin a database transaction
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction?.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        public async Task RollbackAsync()
        {
            try
            {
                await _transaction?.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
