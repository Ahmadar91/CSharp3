using System;

namespace BL.Models
{
    public class PlayListItemDto
    {
        public Guid Id { get; set; }
        public string FullPath { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public Guid AlbumId { get; set; }
    }
}