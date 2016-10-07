using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConcurrencyBook.Samples
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private int GetItemTop(int height, int index)
		{
			return index * (height + 8) + 8;
		}

		private void RefreshContent(WallpapersInfo info)
		{
			_resultPanel.Controls.Clear();
			_resultPanel.Controls.AddRange(
					info
						.Wallpapers
						.SelectMany(
							(wallpaper, i) =>
								new Control[]
								{
									new PictureBox
									{
										Left = 4,
										Image = wallpaper.Thumbnail,
										AutoSize = true,
										Top = GetItemTop(wallpaper.Thumbnail.Height, i)
									},
									new Label
									{
										Left = wallpaper.Thumbnail.Width + 8,
										Top = GetItemTop(wallpaper.Thumbnail.Height, i),
										Text = wallpaper.Description,
										AutoSize = true
									}
								})
						.ToArray()
					);
			_timeLabel.Text = string.Format("Time: {0}ms", info.Milliseconds);
		}

		private void _loadSyncBtn_Click(object sender, System.EventArgs e)
		{
			var info = Loader.SyncLoad();
			RefreshContent(info);
		}

		private void _loadTaskBtn_Click(object sender, System.EventArgs e)
		{
			var info = Loader.TaskLoad();
			info.ContinueWith(task => RefreshContent(task.Result),
				CancellationToken.None,
				TaskContinuationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext());
		}

		private async void _loadAsyncBtn_Click(object sender, System.EventArgs e)
		{
			var info = await Loader.AsyncLoad();
			RefreshContent(info);
		}

		private void _iteratorBtn_Click(object sender, System.EventArgs e)
		{
			var info = Loader.IteratorLoad();
			RefreshContent(info);
		}
	}
}
