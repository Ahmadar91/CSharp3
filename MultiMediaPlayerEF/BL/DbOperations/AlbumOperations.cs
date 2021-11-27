using BL.DbOperations.Interface;
using DAL.Core;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using BL.Models;

namespace BL.DbOperations
{
    public class AlbumOperations : IAlbumDbOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlbumOperations(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<AlbumDto> GetAll()
        {
            var albums = _unitOfWork.Album.All();
            return _mapper.Map(albums);
        }

        public AlbumDto GetById(Guid id)
        {
            var album = _unitOfWork.Album.GetById(id);
            return _mapper.Map(album);
        }

        public bool Add(AlbumDto entity)
        {
            if (entity != null)
            {
                var res = _unitOfWork.Album.Add(_mapper.Map(entity));
                if (res)
                {
                    _unitOfWork.Complete();
                }

                return res;
            }
            return false;
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

        public bool Edit(AlbumDto entity)
        {
            var result = _unitOfWork.Album.Upsert(_mapper.Map(entity));
            if (result)
            {
                _unitOfWork.Complete();
            }
            return result;
        }
        public IEnumerable<AlbumDto> Find(Expression<Func<Album, bool>> predicate)
        {
            return _mapper.Map(_unitOfWork.Album.Find(predicate));
        }
    }
}