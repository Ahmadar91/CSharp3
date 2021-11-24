using System.Collections.Generic;
using BL.Models;

namespace BL.Interfaces
{
    public interface IDirectoryUtils
    {
        public List<DirectoryItem> GetLogicalDrives();
        public List<DirectoryItem> GetDirectoryContent(string fullPath);
    }
}