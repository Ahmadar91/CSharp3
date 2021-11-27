using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Models;

namespace BL.DbOperations.Interface
{
    public interface IPlayListDbOperation
    {
        IEnumerable<PlayListItem> GetAll();
        PlayListItem GetById(Guid id);
        bool Add(PlayListItem entity);
        bool Delete(Guid id);
        bool Edit(PlayListItem entity);
        IEnumerable<PlayListItem> Find(Expression<Func<PlayListItem, bool>> predicate);
    }
}