using BL.Directory;

namespace BL.Models
{
	public class DirectoryItem
	{
		public string FullPath { get; set; }

		public string FileName => Type == DirectoryItemType.Drive ? FullPath : DirectoryUtils.GetFileFolderName(FullPath);

		public DirectoryItemType Type { get; set; }

		public MediaType MediaType { get; set; }
	}
}
