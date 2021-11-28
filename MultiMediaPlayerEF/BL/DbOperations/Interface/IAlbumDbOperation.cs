using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BL.Models;
using DAL.Core;
using DAL.Models;

namespace BL.DbOperations.Interface
{
    public interface IAlbumDbOperation
    {
        /// <summary>
        /// Get all the Albums
        /// </summary>
        /// <returns></returns>
        IEnumerable<AlbumDto> GetAll();
        /// <summary>
        /// Get an Album by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AlbumDto GetById(Guid id);
        /// <summary>
        /// Add an Album
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(AlbumDto entity);
        /// <summary>
        /// Delete an Album
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(Guid id);
        /// <summary>
        /// Edit an album
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Edit(AlbumDto entity);
        /// <summary>
        /// Find An Album based on a condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<AlbumDto> Find(Expression<Func<Album, bool>> predicate);
    }
}