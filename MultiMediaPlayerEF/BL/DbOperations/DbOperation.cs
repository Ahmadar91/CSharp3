using BL.DbOperations.Interface;
using DAL.Core;

namespace BL.DbOperations
{
    public class DbOperation 
    {
        private readonly IUnitOfWork _unitOfWork;
        public IAlbumDbOperation AlbumDbOperation { get; set; }
        public IPlayListDbOperation PlayListDbOperation { get; set; }
        public DbOperation()
        {
            _unitOfWork = new UnitOfWork();
            AlbumDbOperation = new AlbumOperations(_unitOfWork);
            PlayListDbOperation = new PlayListOperation(_unitOfWork);
        }
    }
}