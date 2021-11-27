using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BL.Models;
using DAL.Models;

namespace BL.DbOperations.Interface
{
    public interface IPlayListDbOperation
    {
        IEnumerable<PlayListItemDto> GetAll();
        PlayListItemDto GetById(Guid id);
        bool Add(PlayListItemDto entity);
        bool Delete(Guid id);
        bool Edit(PlayListItemDto entity);
        IEnumerable<PlayListItemDto> Find(Expression<Func<PlayListItem, bool>> predicate);
    }
}