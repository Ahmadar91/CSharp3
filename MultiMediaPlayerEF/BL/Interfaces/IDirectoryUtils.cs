using System.Collections.Generic;
using BL.Models;

namespace BL.Interfaces
{
    public interface IDirectoryUtils
    {
        /// <summary>
        /// Get all Logical Drives
        /// </summary>
        /// <returns></returns>
        public List<DirectoryItem> GetLogicalDrives();
        /// <summary>
        /// Get Directory content
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public List<DirectoryItem> GetDirectoryContent(string fullPath);
    }
}