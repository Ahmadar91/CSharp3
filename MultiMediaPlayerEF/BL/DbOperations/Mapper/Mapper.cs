using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BL.DbOperations.Interface;
using BL.Models;
using DAL.Models;

namespace BL.DbOperations.Mapper
{
    public class Mapper : IMapper
    {
        public AlbumDto Map(Album album)
        {
            return new AlbumDto
            {
                Id = album.Id,
                Description = album.Description,
                PlayList = new ObservableCollection<PlayListItemDto>(album.PlayList.Select(item => new PlayListItemDto
                {
                    Id = item.Id,
                    FullPath = item.FullPath,
                    Description = item.Description,
                    FileName = item.FileName,
                    AlbumId = item.AlbumId
                })),
                Count = album.Count
            };
        }

        public Album Map(AlbumDto albumDto)
        {
            return new Album
            {
                Id = albumDto.Id,
                Description = albumDto.Description,
                PlayList = (ICollection<PlayListItem>)albumDto.PlayList.Select(item => new PlayListItemDto
                {
                    Id = item.Id,
                    FullPath = item.FullPath,
                    Description = item.Description,
                    FileName = item.FileName,
                    AlbumId = item.AlbumId
                }),
                Count = albumDto.Count
            };
        }

        public PlayListItemDto Map(PlayListItem playListItem)
        {
            return new PlayListItemDto
            {
                Id = playListItem.Id,
                FullPath = playListItem.FullPath,
                Description = playListItem.Description,
                FileName = playListItem.FileName,
                AlbumId = playListItem.AlbumId
            };
        }

        public PlayListItem Map(PlayListItemDto playListItemDto)
        {
            return new PlayListItem
            {
                Id = playListItemDto.Id,
                FullPath = playListItemDto.FullPath,
                Description = playListItemDto.Description,
                FileName = playListItemDto.FileName,
                AlbumId = playListItemDto.AlbumId,
            };
        }

        public IEnumerable<Album> Map(IEnumerable<AlbumDto> albumDtos)
        {
            return albumDtos.Select(Map);
        }

        public IEnumerable<AlbumDto> Map(IEnumerable<Album> albums)
        {
            return albums.Select(Map);
        }

        public IEnumerable<PlayListItem> Map(IEnumerable<PlayListItemDto> playListItems)
        {
            return playListItems.Select(Map);
        }

        public IEnumerable<PlayListItemDto> Map(IEnumerable<PlayListItem> playListItemDtos)
        {
            return playListItemDtos.Select(Map);
        }
    }
}