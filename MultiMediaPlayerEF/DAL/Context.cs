using System.IO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class Context : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<PlayListItem> PlayListItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ahmad\Desktop\CSharp3\MultiMediaPlayerEF\DAL\MultiMedia.mdf;Integrated Security=True");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}