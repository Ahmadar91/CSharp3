using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using BL.Interfaces;

namespace BL.Directory
{
    public class DirectoryUtils : IDirectoryUtils
    {
        public DirectoryUtils() { }

        public List<DirectoryItem> GetLogicalDrives()
        {
            return System.IO.Directory.GetLogicalDrives().Select(x => new DirectoryItem()
            {
                FullPath = x,
                Type = DirectoryItemType.Drive,
            }).ToList();
        }
        /// <summary>
        /// Try and get directories from the folder
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public List<DirectoryItem> GetDirectoryContent(string fullPath)
        {
            var items = new List<DirectoryItem>();


            // Try and get directories from the folder

            try
            {
                var dirs = System.IO.Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    items.AddRange(dirs.Select(x => new DirectoryItem()
                    {
                        FullPath = x,
                        Type = DirectoryItemType.Folder
                    }));
            }
            catch (Exception e)
            {
                throw new Exception($"IO Exception: {e.Message}");

            }
            try
            {
                var fs = System.IO.Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                    items.AddRange(fs.Select(y => new DirectoryItem()
                    {
                        FullPath = y,
                        Type = DirectoryItemType.File
                    }));
            }
            catch (Exception e)
            {
                throw new Exception($"IO Exception: {e.Message}");
            }
            return items;
        }
    }
}