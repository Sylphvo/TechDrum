using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TechDrum.Contract.Repository.Data
{
    public interface IRepository<T> where T : class, new()
    {
        void Add(T entity);
        void AddRange(ICollection<T> entity);
        void Update(T entity);
        void UpdateRange(ICollection<T> entity);
        void Delete(T entity);
        void DeleteRange(ICollection<T> entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);

        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetByKey(object key);
    }
}
