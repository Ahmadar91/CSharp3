using BL.DbOperations.Interface;
using DAL.Core;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BL.DbOperations
{
    public class AlbumOperations : IAlbumDbOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        public AlbumOperations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Album> GetAll()
        {
            return _unitOfWork.Album.All();
        }

        public Album GetById(Guid id)
        {
            return _unitOfWork.Album.GetById(id);
        }

        public bool Add(Album entity)
        {
            var res = _unitOfWork.Album.Add(entity);
            if (res)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public bool Delete(Guid id)
        {
            var result = _unitOfWork.Album.Delete(id);
            if (result)
            {
                _unitOfWork.Complete();
            }
            return result;
        }

        public bool Edit(Album entity)
        {
            var result = _unitOfWork.Album.Upsert(entity);
            if (result)
            {
                _unitOfWork.Complete();
            }
            return result;
        }
        public IEnumerable<Album> Find(Expression<Func<Album, bool>> predicate)
        {
            return _unitOfWork.Album.Find(predicate);
        }
    }
}