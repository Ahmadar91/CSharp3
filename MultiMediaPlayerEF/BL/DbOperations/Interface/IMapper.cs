using System.Collections.Generic;
using BL.Models;
using DAL.Models;

namespace BL.DbOperations.Interface
{
    public interface IMapper
    {
        AlbumDto Map(Album album);
        Album Map(AlbumDto albumDto);
        PlayListItemDto Map(PlayListItem playListItem);
        PlayListItem Map(PlayListItemDto playListItemDto);
        IEnumerable<Album> Map(IEnumerable<AlbumDto> albumDtos);
        IEnumerable<AlbumDto> Map(IEnumerable<Album> albums);
        IEnumerable<PlayListItem> Map(IEnumerable<PlayListItemDto> playListItems);
        IEnumerable<PlayListItemDto> Map(IEnumerable<PlayListItem> playListItemDtos);
    }
}
