using MvcTemplate.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcTemplate.Core
{
    //https://dotblogs.com.tw/gelis/2014/09/08/146492
    /// <summary>
    /// 實作Entity Framework Unit Of Work的class
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly IDbContextFactory _databaseFactory;
        private readonly DbContext _context;

        private bool _disposed;
        private Hashtable _repositories;

        /// <summary>
        /// 設定此Unit of work(UOF)的Context。
        /// </summary>
        /// <param name="context">設定UOF的context</param>
        public EFUnitOfWork(IDbContextFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _context = _context ?? _databaseFactory.GetDbContext();
        }

        /// <summary>
        /// 儲存所有異動。
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// 清除此Class的資源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清除此Class的資源。
        /// </summary>
        /// <param name="disposing">是否在清理中？</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        /// <summary>
        /// 取得某一個Entity的Repository。
        /// 如果沒有取過，會initialise一個
        /// 如果有就取得之前initialise的那個。
        /// </summary>
        /// <typeparam name="T">此Context裡面的Entity Type</typeparam>
        /// <returns>Entity的Repository</returns>
        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(DBRepository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), _databaseFactory);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}