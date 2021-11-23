using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Directory
{
	public class DirectoryUtils
	{
		public DirectoryUtils() { }

		public List<DirectoryItem> GetLogicalDirves()
		{
			return System.IO.Directory.GetLogicalDrives().Select(x => new DirectoryItem()
			{
				FullPath = x,
				Type = DirectoryItemType.Drive,
			}).ToList();
		}

		/// <summary>
		/// Find the file or folder name from a full path
		/// </summary>
		/// <param name="path">The full path</param>
		/// <returns></returns>
		public static string GetFileFolderName(string path)
		{
			// If we have no path, return empty
			if (string.IsNullOrEmpty(path))
				return string.Empty;

			// Make all slashes back slashes
			var normalizedPath = path.Replace('/', '\\');

			// Find the last backslash in the path
			var lastIndex = normalizedPath.LastIndexOf('\\');

			// If we don't find a backslash, return the path itself
			if (lastIndex <= 0)
				return path;

			// Return the name after the last back slash
			return path.Substring(lastIndex + 1);
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