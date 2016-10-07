using System.Drawing;

namespace ConcurrencyBook.Samples
{
	public class WallpaperInfo
	{
		private readonly Image _thumbnail;
		private readonly string _description;

		public WallpaperInfo(Image thumbnail, string description)
		{
			_thumbnail = thumbnail;
			_description = description;
		}

		public Image Thumbnail
		{
			get { return _thumbnail; }
		}

		public string Description
		{
			get { return _description; }
		}
	}
}