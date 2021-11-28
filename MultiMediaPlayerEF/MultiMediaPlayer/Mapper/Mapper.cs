using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BL.Models;
using DAL.Models;
using MultiMediaPlayer.ViewModels;

namespace MultiMediaPlayer.Mapper
{
    public interface IMapper
    {/// <summary>
    /// Map a AlbumViewModel to AlbumDTO
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
        AlbumDto Map(AlbumViewModel album);
    /// <summary>
    /// Map a AlbumDTO to AlbumBViewModel
    /// </summary>
    /// <param name="albumDto"></param>
    /// <returns></returns>
        AlbumViewModel Map(AlbumDto albumDto);
    /// <summary>
    /// Map a PlayListIViewModel to PlaylistItemDTO
    /// </summary>
    /// <param name="playListView"></param>
    /// <returns></returns>
        PlayListItemDto Map(PlayListViewModel playListView);
    /// <summary>
    /// Map PlayListViewDTO to PlayListViewModel
    /// </summary>
    /// <param name="playListItemDto"></param>
    /// <returns></returns>
        PlayListViewModel Map(PlayListItemDto playListItemDto);
        /// <summary>
        /// Map a IEnumerable AlbumDTO to IEnumerable AlbumViewModel
        /// </summary>
        /// <param name="albumDtos"></param>
        /// <returns></returns>
        IEnumerable<AlbumViewModel> Map(IEnumerable<AlbumDto> albumDtos);
        /// <summary>
        /// Map IEnumerable AlbumViewModel to AlbumDTO
        /// </summary>
        /// <param name="albums"></param>
        /// <returns></returns>
        IEnumerable<AlbumDto> Map(IEnumerable<AlbumViewModel> albums);
        /// <summary>
        /// Map IEnumerable PlayListItemDTO to IEnumerable PlayListViewModel
        /// </summary>
        /// <param name="playListItems"></param>
        /// <returns></returns>
        IEnumerable<PlayListViewModel> Map(IEnumerable<PlayListItemDto> playListItems);
        /// <summary>
        /// IEnumerable PlaylistViewModel to IEnumerablePlayListItemDTO
        /// </summary>
        /// <param name="playListItemDtos"></param>
        /// <returns></returns>
        IEnumerable<PlayListItemDto> Map(IEnumerable<PlayListViewModel> playListItemDtos);
    }
    public class Mapper : IMapper
    {
        public AlbumDto Map(AlbumViewModel album)
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
        public AlbumViewModel Map(AlbumDto albumDto)
        {
            return new AlbumViewModel
            {
                Id = albumDto.Id,
                Description = albumDto.Description,
                PlayList = new ObservableCollection<PlayListViewModel>(albumDto.PlayList.Select(item => new PlayListViewModel
                {
                    Id = item.Id,
                    FullPath = item.FullPath,
                    Description = item.Description,
                    FileName = item.FileName,
                    AlbumId = item.AlbumId,
                    LoadedImage = LoadThumbnail(item.FullPath)
                })),
                Count = albumDto.Count
            };
        }

        public PlayListItemDto Map(PlayListViewModel playListItem)
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

        public PlayListViewModel Map(PlayListItemDto playListItemDto)
        {
            return new PlayListViewModel
            {
                Id = playListItemDto.Id,
                FullPath = playListItemDto.FullPath,
                Description = playListItemDto.Description,
                FileName = playListItemDto.FileName,
                AlbumId = playListItemDto.AlbumId,
            };
        }

        public IEnumerable<AlbumViewModel> Map(IEnumerable<AlbumDto> albumDtos)
        {
            return albumDtos.Select(Map);
        }

        public IEnumerable<AlbumDto> Map(IEnumerable<AlbumViewModel> albums)
        {
            return albums.Select(Map);
        }

        public IEnumerable<PlayListViewModel> Map(IEnumerable<PlayListItemDto> playListItems)
        {
            return playListItems.Select(Map);
        }

        public IEnumerable<PlayListItemDto> Map(IEnumerable<PlayListViewModel> playListItemDtos)
        {
            return playListItemDtos.Select(Map);
        }
        private BitmapImage LoadThumbnail(string argFullPath)
        {
            if (argFullPath.EndsWith("wav", StringComparison.CurrentCultureIgnoreCase) || argFullPath.EndsWith("mp4", StringComparison.CurrentCultureIgnoreCase))
            {
                return GetThumbnail(argFullPath, 500, 500);
            }
            else
            {
                return new BitmapImage(new Uri(argFullPath));
            }
        }
        /// <summary>
        /// Get Thumbnail for Video
        /// </summary>
        /// <param name="mediaFile"></param>
        /// <param name="waitTime"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private BitmapImage GetThumbnail(string mediaFile, int waitTime, int position)
        {
            MediaPlayer player = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };
            player.Open(new Uri(mediaFile));
            player.Pause();
            player.Position = TimeSpan.FromMilliseconds(position);
            System.Threading.Thread.Sleep(waitTime);
            RenderTargetBitmap rtb = new RenderTargetBitmap(120, 90, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawVideo(player, new Rect(0, 0, 120, 90));
            }
            rtb.Render(dv);
            BitmapFrame frame = BitmapFrame.Create(rtb).GetCurrentValueAsFrozen() as BitmapFrame;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(frame as BitmapFrame);
            MemoryStream memoryStream = new MemoryStream();
            encoder.Save(memoryStream);
            memoryStream.GetBuffer();
            player.Close();
            var Image = new BitmapImage();
            using (var stream = new MemoryStream(memoryStream.GetBuffer()))
            {
                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = stream;
                Image.EndInit();
            }

            Image.Freeze();
            return Image;
        }
    }
}