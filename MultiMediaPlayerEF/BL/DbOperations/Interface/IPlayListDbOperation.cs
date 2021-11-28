using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BL.Models;
using DAL.Models;

namespace BL.DbOperations.Interface
{
    public interface IPlayListDbOperation
    {/// <summary>
    /// Get All PlayListItems
    /// </summary>
    /// <returns></returns>
        IEnumerable<PlayListItemDto> GetAll();
    /// <summary>
    /// Get PlayListItem by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
        PlayListItemDto GetById(Guid id);
    /// <summary>
    /// Add a playListItem
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
        bool Add(PlayListItemDto entity);
    /// <summary>
    /// Delete a playListItem
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
        bool Delete(Guid id);
    /// <summary>
    /// Edit a playListItem
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
        bool Edit(PlayListItemDto entity);
    /// <summary>
    /// Find a playList item based on a condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
        IEnumerable<PlayListItemDto> Find(Expression<Func<PlayListItem, bool>> predicate);
    }
}