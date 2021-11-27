using BL.DbOperations.Interface;
using BL.Models;
using DAL.Core;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BL.DbOperations
{
    public class PlayListOperation : IPlayListDbOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlayListOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<PlayListItemDto> GetAll()
        {
            return _mapper.Map(_unitOfWork.PlayList.All());
        }

        public PlayListItemDto GetById(Guid id)
        {
            return _mapper.Map(_unitOfWork.PlayList.GetById(id));
        }

        public bool Add(PlayListItemDto entity)
        {
            var result = _unitOfWork.PlayList.Add(_mapper.Map(entity));
            if (result)
            {
                _unitOfWork.Complete();
            }
            return result;
        }

        public bool Delete(Guid id)
        {
            var result = _unitOfWork.PlayList.Delete(id);

            if (result)
            {
                _unitOfWork.Complete();
            }
            return result;
        }

        public bool Edit(PlayListItemDto entity)
        {
            var result = _unitOfWork.PlayList.Upsert(_mapper.Map(entity));
            if (result)
            {
                _unitOfWork.Complete();
            }
            return result;
        }

        public IEnumerable<PlayListItemDto> Find(Expression<Func<PlayListItem, bool>> predicate)
        {
            return _mapper.Map(_unitOfWork.PlayList.Find(predicate));
        }

    }
}