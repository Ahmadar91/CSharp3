using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BL.DbOperations
{
    public class AlbumOperations
    {
        public void DbTest()
        {
            var db =  new Context();
     
            db.Database.EnsureCreated();
            var album = new Album
            {
                Description = "ada",
                PlayList = new List<PlayListItem>()
                {
                    new PlayListItem
                    {
                        FullPath = "adad",
                        Description = null,
                        FileName = null
                    }
                },
                Count = 2
            };
            db.Albums.Add(album);
            db.SaveChanges();
            db.Albums.Load(); 
            
            var test = db.Set<Album>().ToList();
          var test2 =  db.Albums.Local.ToObservableCollection();

        }
    }
}