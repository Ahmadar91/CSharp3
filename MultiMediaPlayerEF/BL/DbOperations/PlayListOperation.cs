﻿using BL.DbOperations.Interface;
using DAL.Core;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BL.DbOperations
{
    public class PlayListOperation : IPlayListDbOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlayListOperation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlayListItem> GetAll()
        {
            return _unitOfWork.PlayList.All();
        }

        public PlayListItem GetById(Guid id)
        {
            return _unitOfWork.PlayList.GetById(id);
        }

        public bool Add(PlayListItem entity)
        {
            var result = _unitOfWork.PlayList.Add(entity);
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

        public bool Edit(PlayListItem entity)
        {
            var result = _unitOfWork.PlayList.Upsert(entity);
            if (result)
            {
                _unitOfWork.Complete();
            }
            return result;
        }

        public IEnumerable<PlayListItem> Find(Expression<Func<PlayListItem, bool>> predicate)
        {
            return _unitOfWork.PlayList.Find(predicate);
        }
    }
}