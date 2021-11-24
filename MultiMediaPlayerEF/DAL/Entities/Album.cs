using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PlayListItem> PlayList { get;  set; }
        public int Count { get; set; }
    }
}