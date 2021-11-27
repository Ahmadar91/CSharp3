using BL.DbOperations.Interface;
using DAL.Core;

namespace BL.DbOperations
{
    public class DbOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IAlbumDbOperation AlbumDbOperation { get; set; }
        public IPlayListDbOperation PlayListDbOperation { get; set; }
        public DbOperation()
        {
            _mapper = new Mapper.Mapper();
            _unitOfWork = new UnitOfWork();
            AlbumDbOperation = new AlbumOperations(_unitOfWork, _mapper);
            PlayListDbOperation = new PlayListOperation(_unitOfWork, _mapper);
        }
    }
}