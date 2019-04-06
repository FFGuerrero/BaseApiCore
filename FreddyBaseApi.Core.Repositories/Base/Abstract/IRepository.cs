using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FreddyBaseApi.Core.Repositories.Base.Abstract
{
    public interface IRepository<T>
    {
        #region Methods
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> AllAsNoTracking { get; }
        IQueryable<T> AllOrderedByKey { get; }
        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        T Find(params object[] entityId);
        T Find(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        T Max(Expression<Func<object, T>> predicate, bool asNoTracking = false);
        void Update(T entity);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(int id);
        void Remove(T entity);
        void Remove(params int[] entities);
        void Remove(params T[] entities);
        void RemoveAll(Expression<Func<T, bool>> predicate);
        void RemoveAll(IEnumerable<int> idList);
        void RemoveAll(IEnumerable<T> entities);
        IEnumerable<T> ExecuteSqlQuery<T>(string sql, DbParameter[] dbParameters) where T : class;
        int ExecuteSqlCommand(string sql, DbParameter[] dbParameters);
        bool Exists(Func<T, bool> predicate);
        #endregion

        #region Async Methods
        Task Reload(T entity);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false);

        Task<T> FindAsync(params object[] entityId);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> MaxAsync(Expression<Func<object, T>> predicate);
        //TODO: The AddAsync method should return Task instead Task<T>
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveAllAsync(IEnumerable<T> entities);
        Task<int> ExecuteSqlCommandAsync(string sql, DbParameter[] dbParameters);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task AddReference(T entity, Expression<Func<T, object>> navigationProperty);
        #endregion
    }
}
