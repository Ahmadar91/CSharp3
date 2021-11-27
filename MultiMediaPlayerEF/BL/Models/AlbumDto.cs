using System;
using System.Collections.ObjectModel;

namespace BL.Models
{
    public class AlbumDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ObservableCollection<PlayListItemDto> PlayList { set; get; }
        public int Count { get; set; }
    }
}