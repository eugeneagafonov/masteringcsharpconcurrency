namespace ConcurrencyBook.Samples
{
	public class WallpapersInfo
	{
		private readonly long _milliseconds;
		private readonly WallpaperInfo[] _wallpapers;

		public WallpapersInfo(long milliseconds, WallpaperInfo[] wallpapers)
		{
			_milliseconds = milliseconds;
			_wallpapers = wallpapers;
		}

		public long Milliseconds
		{
			get { return _milliseconds; }
		}

		public WallpaperInfo[] Wallpapers
		{
			get { return _wallpapers; }
		}
	}
}