using System.Threading.Tasks;

namespace DAL.Core
{
    public interface IUnitOfWork 
    {
        IAlbumRepository Album { get; }
        IPlayListItemRepository PlayList { get; }

        void Complete();
    }
}