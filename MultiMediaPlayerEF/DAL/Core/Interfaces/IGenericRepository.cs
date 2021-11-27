using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Core
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> All();
        T GetById(Guid id);
        bool Add(T entity);
        bool Delete(Guid id);
        bool Upsert(T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}