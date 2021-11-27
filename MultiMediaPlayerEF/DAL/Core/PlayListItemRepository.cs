using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace DAL.Core
{
    public class PlayListItemRepository : GenericRepository<PlayListItem>, IPlayListItemRepository
    {
        public PlayListItemRepository(Context context) : base(context)
        {
        }
        public override IEnumerable<PlayListItem> All()
        {
            try
            {
                return dbSet.ToList();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "{Repo} All function error", typeof(UserRepository));
                return new List<PlayListItem>();
            }
        }

        public override bool Upsert(PlayListItem entity)
        {
            try
            {
                var existingUser = dbSet.Where(x => x.Id == entity.Id)?.FirstOrDefault();

                if (existingUser == null)
                    return Add(entity);

                existingUser.Description = entity.Description;
                existingUser.FileName = entity.FileName;
                existingUser.FullPath = entity.FullPath;
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
                var exist = dbSet.Where(x => x.Id == id)?.FirstOrDefault();

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