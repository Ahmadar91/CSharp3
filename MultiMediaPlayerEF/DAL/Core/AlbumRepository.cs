using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Core
{
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(Context context) : base(context)
        {

        }

        public override IEnumerable<Album> All()
        {
            try
            {
                return  dbSet.ToList();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "{Repo} All function error", typeof(UserRepository));
                return new List<Album>();
            }
        }

        public override bool Upsert(Album entity)
        {
            try
            {
                var existingUser =  dbSet.Where(x => x.Id == entity.Id)?.FirstOrDefault();

                if (existingUser == null)
                    return  Add(entity);

                existingUser.Description = entity.Description;
                existingUser.PlayList = entity.PlayList;
                existingUser.Count = entity.Count;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool Delete(Guid id)
        {
            try
            {
                var exist =  dbSet.Where(x => x.Id == id)?.FirstOrDefault();

                if (exist == null) return false;

                dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
