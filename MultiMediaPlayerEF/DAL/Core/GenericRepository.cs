using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Core
{
    public class GenericRepository<T>  :IGenericRepository<T> where T : class
    {
        protected Context context;
        internal DbSet<T> dbSet;
        public GenericRepository(Context context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public virtual IEnumerable<T> All()
        {
            throw new NotImplementedException();
        }

        public virtual T GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public virtual bool Add(T entity)
        {
             dbSet.Add(entity);
             return true;
        }

        public virtual bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual bool Upsert(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return  dbSet.Where(predicate).ToList();
        }
    }
}