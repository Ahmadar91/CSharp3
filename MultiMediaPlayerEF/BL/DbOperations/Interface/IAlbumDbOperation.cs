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
        IEnumerable<AlbumDto> GetAll();
        AlbumDto GetById(Guid id);
        bool Add(AlbumDto entity);
        bool Delete(Guid id);
        bool Edit(AlbumDto entity);
        IEnumerable<AlbumDto> Find(Expression<Func<Album, bool>> predicate);
    }
}