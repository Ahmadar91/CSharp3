using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Core;
using DAL.Models;

namespace BL.DbOperations.Interface
{
    public interface IAlbumDbOperation
    {
        IEnumerable<Album> GetAll();
        Album GetById(Guid id);
        bool Add(Album entity);
        bool Delete(Guid id);
        bool Edit(Album entity);
        IEnumerable<Album> Find(Expression<Func<Album, bool>> predicate);
    }
}