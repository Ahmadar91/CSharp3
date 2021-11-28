using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Core
{
    public interface IGenericRepository<T> where T : class
    {/// <summary>
    /// Get All T 
    /// </summary>
    /// <returns></returns>
        IEnumerable<T> All();
    /// <summary>
    /// Get T by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
        T GetById(Guid id);
            /// <summary>
            /// Add T 
            /// </summary>
            /// <param name="entity"></param>
            /// <returns></returns>
        bool Add(T entity);
            /// <summary>
            /// Delete T
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
        bool Delete(Guid id);
            /// <summary>
            /// Edit T
            /// </summary>
            /// <param name="entity"></param>
            /// <returns></returns>
        bool Upsert(T entity);
            /// <summary>
            /// Find T based on a condition
            /// </summary>
            /// <param name="predicate"></param>
            /// <returns></returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}