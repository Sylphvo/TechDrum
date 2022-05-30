using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TechDrum.Repository.Data
{
    public abstract class Repository<T> where T : class, new()
    {
        private readonly AppDbContext _dbContext;
        private DbSet<T> _dbSet;

        protected DbSet<T> DbSet
        {
            get
            {
                if (_dbSet != null)
                {
                    return _dbSet;
                }

                _dbSet = _dbContext.Set<T>();

                return _dbSet;
            }
        }

        protected Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get

        public virtual T GetByKey(object key)
        {
            return DbSet.Find(key);
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            var query = DbSet.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        #endregion

        #region Add

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void AddRange(ICollection<T> entities)
        {
            DbSet.AddRange(entities);
        }

        #endregion

        #region Update

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(ICollection<T> entities)
        {
            DbSet.UpdateRange(entities);
        }

        #endregion

        #region Delete

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void DeleteRange(ICollection<T> entity)
        {
            DbSet.RemoveRange(entity);
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            var entities = Get(predicate);
            if (entities.Any())
            {
                DbSet.RemoveRange(entities);
            }
        }

        #endregion
    }
}
