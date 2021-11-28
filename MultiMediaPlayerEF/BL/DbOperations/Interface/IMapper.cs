using System.Collections.Generic;
using BL.Models;
using DAL.Models;

namespace BL.DbOperations.Interface
{
    public interface IMapper
    {/// <summary>
    ///Map an Album To AlbumDTO
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
        AlbumDto Map(Album album);
    /// <summary>
    /// Map an AlbumDTO to Album
    /// </summary>
    /// <param name="albumDto"></param>
    /// <returns></returns>
        Album Map(AlbumDto albumDto);
    /// <summary>
    /// Map a PlaylistItem to PlayListItemDTO
    /// </summary>
    /// <param name="playListItem"></param>
    /// <returns></returns>
        PlayListItemDto Map(PlayListItem playListItem);
    /// <summary>
    /// Map a PlaylistItemDTO to PlayListItem
    /// </summary>
    /// <param name="playListItemDto"></param>
    /// <returns></returns>
        PlayListItem Map(PlayListItemDto playListItemDto);
        /// <summary>
        /// Map an IEnumerable AlbumDTO to IEnumerable Album
        /// </summary>
        /// <param name="albumDtos"></param>
        /// <returns></returns>
        IEnumerable<Album> Map(IEnumerable<AlbumDto> albumDtos);
        /// <summary>
        /// Map an IEnumerable Album to IEnumerable AlbumDTO
        /// </summary>
        /// <param name="albums"></param>
        /// <returns></returns>
        IEnumerable<AlbumDto> Map(IEnumerable<Album> albums);
        /// <summary>
        /// Map an IEnumerable PlayListItemDTO to IEnumerable PlaylistItem
        /// </summary>
        /// <param name="playListItems"></param>
        /// <returns></returns>
        IEnumerable<PlayListItem> Map(IEnumerable<PlayListItemDto> playListItems);
        /// <summary>
        /// Map a IEnumerable PlayListItem to IEnumerable PlayListItemDTO
        /// </summary>
        /// <param name="playListItemDtos"></param>
        /// <returns></returns>
        IEnumerable<PlayListItemDto> Map(IEnumerable<PlayListItem> playListItemDtos);
    }
}
