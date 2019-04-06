using FreddyBaseApi.Core.Repositories.Base.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FreddyBaseApi.Core.Repositories.Base.Concrete
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Context

        protected DbContext Context { get; set; }

        #endregion

        #region Constructors
        public RepositoryBase()
        {

        }
        public RepositoryBase(DbContext dbContext)
        {
            Context = dbContext;
        }
        #endregion

        #region Abstract Properties

        protected abstract string FriendlyName { get; }
        protected abstract Func<T, object> KeySelector { get; }
        protected abstract Func<T, object> OrderBySelector { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a all records
        /// </summary>
        public IQueryable<T> GetAll() => Context.Set<T>();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> entity = Context.Set<T>();

            foreach (var item in includes)
            {
                entity = Context.Set<T>().Include(item);
            }

            return entity;
        }

        /// <summary>
        /// Gets all records as no tracking
        /// </summary>
        public IQueryable<T> AllAsNoTracking => Context.Set<T>().AsNoTracking();

        /// <summary>
        /// Get ordered list by default
        /// </summary>
        public IQueryable<T> AllOrderedByKey => GetAll().OrderBy(OrderBySelector).AsQueryable();

        /// <summary>
        /// Get a list of entities by a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
            => asNoTracking ? AllAsNoTracking.Where(predicate) : GetAll().Where(predicate);

        /// <summary>
        ///  Get a list of entities based a predicate and include nested properties
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAllBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {

            IQueryable<T> entity = Context.Set<T>();

            foreach (var item in includes)
            {
                entity = entity.Include(item);
            }

            return entity.Where(predicate);
        }

        /// <summary>
        /// Get a single entity by primary key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual T Find(params object[] entityId) => Context.Set<T>().Find(entityId);

        /// <summary>
        /// Get a single entity based a predicate and include nested properties
        /// </summary>
        /// <param name="action"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> entity = Context.Set<T>();

            foreach (var item in includes)
            {
                entity = entity.Include(item);
            }

            return entity.SingleOrDefault(predicate);
        }

        /// <summary>
        /// Get a single record by a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
            => asNoTracking ? AllAsNoTracking.SingleOrDefault(predicate) : GetAll().SingleOrDefault(predicate);

        /// <summary>
        /// Get max value for a property of the object
        /// </summary>
        /// <param name="predicate">The predicate for select the property</param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public virtual T Max(Expression<Func<object, T>> predicate, bool asNoTracking = false)
            => asNoTracking ? AllAsNoTracking.Max(predicate) : GetAll().Max(predicate);

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Update(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChanges();
                LogEntityActivity(string.Format("Entity instance for {0} has been updated: {1}", FriendlyName, entity.ToString()));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Add(T entity)
        {
            try
            {
                Context.Set<T>().Add(entity);
                Context.SaveChanges();
                LogEntityActivity(string.Format("Entity instance for {0} has been created: {1}", FriendlyName, entity.ToString()));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add a list of entities
        /// </summary>
        /// <param name="Entities"></param>
        /// <returns></returns>
        public virtual void AddRange(IEnumerable<T> entities)
        {
            try
            {
                Context.Set<T>().AddRange(entities);
                Context.SaveChanges();

                foreach (T item in entities)
                {
                    LogEntityActivity(string.Format("Entity instance for {0} has been created: {1}", FriendlyName, item.ToString()));
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(int id)
        {
            T entity = Find(id);
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
            LogEntityActivity(string.Format("Entity instance for {0} has been deleted: {1}", FriendlyName, entity.ToString()));
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
            LogEntityActivity(string.Format("Entity instance for {0} has been deleted: {1}", FriendlyName, entity.ToString()));
        }

        /// <summary>
        /// Delete a List on Entity on DB
        /// </summary>
        /// <param name="Entity"></param>
        public virtual void Remove(params T[] entities)
        {
            try
            {
                Context.RemoveRange(entities);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a List on Entity on DB based on entity id list.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Remove(params int[] entities)
        {
            try
            {
                Remove(entities.Select(x => Find(x)).ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a List on Entity on DB based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        public virtual void RemoveAll(Expression<Func<T, bool>> predicate) =>
            Remove(Context.Set<T>().Where(predicate).ToArray());

        /// <summary>
        /// Delete a list of entities
        /// </summary>
        /// <param name="entity"></param>
        public virtual void RemoveAll(IEnumerable<int> idList)
        {
            Context.Set<T>().RemoveRange(idList.Select(x => this.Find(x)));
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a list of entities
        /// </summary>
        /// <param name="entity"></param>
        public virtual void RemoveAll(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
            Context.SaveChanges();
            foreach (T item in entities)
            {
                LogEntityActivity(string.Format("Entity instance for {0} has been deleted: {1}", FriendlyName, item.ToString()));
            }
        }

        /// <summary>
        /// Execute a stored procedure that return an entity resultset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, DbParameter[] dbParameters) where T : class
            => Context.Query<T>().FromSql(sql, dbParameters);

        /// <summary>
        /// Execute an stored procedure and return affected rows
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, DbParameter[] dbParameters) => Context.Database.ExecuteSqlCommand(sql, dbParameters);

        /// <summary>
        /// Verify if an entity exists by predicate.
        /// AsNoTracking because tracking info is no needed here.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool Exists(Func<T, bool> predicate) => Context.Set<T>().AsNoTracking().Any(predicate);

        public void SetLogEntityActivity(Action<string> action)
            => LogEntityActivity = action;

        private void LogTraceEvent(string query)
            => System.Diagnostics.Debug.WriteLine("{0}:{1}", DateTime.Now, query);

        private Action<string> LogEntityActivity = (s) => System.Diagnostics.Debug.WriteLine("{0}:{1}", DateTime.Now, s);

        #endregion

        #region AsyncMethods

        public virtual Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {
            if (asNoTracking)
                return AllAsNoTracking.FirstOrDefaultAsync(predicate);

            return Context.Set<T>().FirstOrDefaultAsync(predicate);
        }


        /// <summary>
        /// Get a single record by primary key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual Task<T> FindAsync(params object[] entityId) => Context.Set<T>().FindAsync(entityId);

        /// <summary>
        /// Get a single record by a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<T> FindAsync(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().SingleOrDefaultAsync(predicate);

        /// <summary>
        /// Get max value for a property of the object
        /// </summary>
        /// <param name="predicate">The predicate for select the property</param>
        /// <returns></returns>
        public virtual Task<T> MaxAsync(Expression<Func<object, T>> predicate)
            => Context.Set<T>().MaxAsync(predicate);

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(T entity)
        {
            try
            {
                Context.Set<T>().Add(entity);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                LogEntityActivity(string.Format("Creando entidad {0}: {1}", FriendlyName, entity.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add a list of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task AddRangeAsync(List<T> entities)
        {
            try
            {
                Context.Set<T>().AddRange(entities);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                foreach (T item in entities)
                {
                    LogEntityActivity(string.Format("Creando entidad {0}: {1}", FriendlyName, item.ToString()));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                await Context.SaveChangesAsync().ConfigureAwait(false);
                LogEntityActivity(string.Format("Actualizando entidad {0}: {1}", FriendlyName, entity.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task RemoveAsync(T entity)
        {
            try
            {
                Context.Set<T>().Remove(entity);
                LogEntityActivity(string.Format("Borrando entidad {0}: {1}", FriendlyName, entity.ToString()));
                await Context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual Task RemoveAllAsync(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Execute an stored procedure and return affected rows
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public Task<int> ExecuteSqlCommandAsync(string sql, DbParameter[] dbParameters)
            => Context.Database.ExecuteSqlCommandAsync(sql, dbParameters);

        /// <summary>
        /// Verify if an entity exists by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().AsNoTracking().AnyAsync(predicate);

        /// <summary>
        /// Reloads the entity from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task Reload(T entity) => Context.Entry(entity).ReloadAsync();

        /// <summary>
        /// Add specific reference of an entity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="navigationProperty">The reference to add</param>
        public virtual Task AddReference(T entity, Expression<Func<T, object>> navigationProperty)
        {
            return Context.Entry(entity).Reference(navigationProperty).LoadAsync();
        }

        #endregion
    }
}
