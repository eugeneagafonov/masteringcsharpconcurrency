using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

// ReSharper disable PossibleNullReferenceException

namespace ConcurrencyBook.Samples
{
	public static class Loader
	{
		private const string _catalogUri = "http://www.bing.com/hpimagearchive.aspx?format=xml&idx=0&n=8&mbl=1&mkt=en-ww";
		private const string _imageUri = "http://bing.com{0}_1920x1080.jpg";

		private static Image GetThumbnail(Stream imageStream)
		{
			using (imageStream)
			{
				var fullBitmap = Image.FromStream(imageStream);
				return new Bitmap(fullBitmap, 192, 108);
			}
		}

		private static Stream DownloadData(this HttpClient client, string uri)
		{
			var response = client.PostAsync(uri, new StringContent(string.Empty)).Result;
			return response.Content.ReadAsStreamAsync().Result;
		}

		private static Task<Stream> DownloadDataAsync(this HttpClient client, string uri)
		{
			Task<HttpResponseMessage> responseTask = client.PostAsync(uri, new StringContent(string.Empty));
			return responseTask.ContinueWith(task => task.Result.Content.ReadAsStreamAsync()).Unwrap();
		}

		private static string DownloadString(this HttpClient client, string uri)
		{
			return client.GetStringAsync(uri).Result;
		}

		public static WallpapersInfo SyncLoad()
		{
			var sw = Stopwatch.StartNew();

			var client = new HttpClient();
			var catalogXmlString = client.DownloadString(_catalogUri);
			var xDoc = XDocument.Parse(catalogXmlString);

			var wallpapers = xDoc
				.Root
				.Elements("image")
				.Select(e =>
					new
					{
						Desc = e.Element("copyright").Value,
						Url = e.Element("urlBase").Value
					})
				.Select(item =>
					new
					{
						item.Desc,
						FullImageData = client.DownloadData(string.Format(_imageUri, item.Url))
					})
				.Select( item =>
					new WallpaperInfo(
						GetThumbnail(item.FullImageData),
						item.Desc))
				.ToArray();
			sw.Stop();

			return new WallpapersInfo(sw.ElapsedMilliseconds, wallpapers);
		}

		public static Task<WallpapersInfo> TaskLoad()
		{
			var sw = Stopwatch.StartNew();

			var downloadBingXmlTask = new HttpClient().GetStringAsync(_catalogUri);

			var parseXmlTask = downloadBingXmlTask.ContinueWith(task =>
			{
				var xmlDocument = XDocument.Parse(task.Result);
				return xmlDocument.Root
					.Elements("image")
					.Select(e =>
						new
						{
							Description = e.Element("copyright").Value,
							Url = e.Element("urlBase").Value
						});
			});

			var downloadImagesTask = parseXmlTask.ContinueWith(task => Task.WhenAll(
				task.Result.Select(item => 
					new HttpClient().DownloadDataAsync(string.Format(_imageUri, item.Url))
						.ContinueWith(downloadTask =>
							new WallpaperInfo(GetThumbnail(downloadTask.Result), item.Description)))))
				.Unwrap();

			return downloadImagesTask.ContinueWith(task =>
			{
				sw.Stop();

				return new WallpapersInfo(sw.ElapsedMilliseconds, task.Result);
			});	
		}

		#region Async

		public static async Task<WallpapersInfo> AsyncLoad()
		{
			var sw = Stopwatch.StartNew();

			var client = new HttpClient();
			var catalogXmlString = await client.GetStringAsync(_catalogUri);
			var xDoc = XDocument.Parse(catalogXmlString);

			var wallpapersTask = xDoc
				.Root
				.Elements("image")
				.Select(e =>
					new
					{
						Description = e.Element("copyright").Value,
						Url = e.Element("urlBase").Value
					})
				.Select(async item =>
					new
					{
						item.Description,
						FullImageData = await client.DownloadDataAsync(string.Format(_imageUri, item.Url))
					});

			var wallpapersItems = await Task.WhenAll(wallpapersTask);

			var wallpapers = wallpapersItems.Select(item => 
				new WallpaperInfo(GetThumbnail(item.FullImageData), item.Description));

			sw.Stop();

			return new WallpapersInfo(sw.ElapsedMilliseconds, wallpapers.ToArray());
		}

		#endregion

		#region Iterator

		private static Task<TResult> ExecuteIterator<TResult>(
			Func<Action<TResult>,IEnumerable<Task>> iteratorGetter)
		{
			return Task.Run(() =>
			{
				var result = default(TResult);
				foreach (var task in iteratorGetter(res => result = res))
					task.Wait();

				return result;
			});
		}

		private static IEnumerable<Task> GetImageIterator(
			string url,
			string desc,
			Action<WallpaperInfo> resultSetter)
		{
			var loadTask =
				new HttpClient().DownloadDataAsync(
					string.Format(_imageUri, url));
			yield return loadTask;

			var thumbTask = Task.FromResult(GetThumbnail(loadTask.Result));
			yield return thumbTask;

			resultSetter(new WallpaperInfo(thumbTask.Result, desc));
		}

		private static IEnumerable<Task> GetWallpapersIterator(
			Action<WallpaperInfo[]> resultSetter)
		{
			var catalogTask =
				new HttpClient().GetStringAsync(_catalogUri);
			yield return catalogTask;

			var xDoc = XDocument.Parse(catalogTask.Result);
			var imagesTask =
				Task.WhenAll(
					xDoc
						.Root
						.Elements("image")
						.Select(
							e =>
								new
								{
									Description = e.Element("copyright").Value,
									Url = e.Element("urlBase").Value
								})
						.Select(
							item =>
								ExecuteIterator<WallpaperInfo>(
									resSetter =>
										GetImageIterator(item.Url, item.Description, resSetter))));
			yield return imagesTask;

			resultSetter(imagesTask.Result);
		}

		public static WallpapersInfo IteratorLoad()
		{
			var sw = Stopwatch.StartNew();

			var wallpapers =
				ExecuteIterator<WallpaperInfo[]>(GetWallpapersIterator)
					.Result;

			sw.Stop();

			return new WallpapersInfo(sw.ElapsedMilliseconds, wallpapers);
		}
		#endregion

	}
}