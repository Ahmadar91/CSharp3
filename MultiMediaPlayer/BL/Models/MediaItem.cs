using System.Net.Mime;
using BL.Models;

namespace BL.Models
{
	public class MediaItem
	{
		public string Id { get; set; }
		public string FileName { get; set; }
		public string Description { get; set; }
		public MediaType Type { get; set; }
		public string FullPath { get; set; }
	}
}