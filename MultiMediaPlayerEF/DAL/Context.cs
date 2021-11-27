using System;
using System.IO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class Context : DbContext
    {
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<PlayListItem> PlayListItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // The Db is available in the MultiMediaPlayerEF\MultiMediaPlayer\bin\Debug\net5.0-windows
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MultiMedia.mdf;Integrated Security=True");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}