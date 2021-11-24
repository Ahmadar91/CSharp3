using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class PlayListItem
    {
        [Key]
        public Guid Id { get; set; }
        public string FullPath { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }

        [ForeignKey("Album_Id")]
        public Guid AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }
}