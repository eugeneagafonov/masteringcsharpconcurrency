using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ConcurrencyBook.Samples
{
	class Program
	{
		private static Label _label;

		[STAThread]
		static void Main(string[] args)
		{
			prepareGUIApplication();
		}

		static void Click(object sender, EventArgs e)
		{
			var t = Task.Factory.StartNew(() =>
			{
				Console.WriteLine("Id: {0}, Is threadpool thread: {1}",
					Thread.CurrentThread.ManagedThreadId,
					Thread.CurrentThread.IsThreadPoolThread);

				Thread.Sleep(TimeSpan.FromSeconds(1));
				_label.Content = new TextBlock {Text = "Hello from TPL task"};
			},
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
//			TaskScheduler.FromCurrentSynchronizationContext());
//			new SynchronousTaskScheduler());

			while (t.Status != TaskStatus.RanToCompletion && t.Status != TaskStatus.Faulted)
			{
				// run message loop
				Application.Current.Dispatcher.Invoke(
					DispatcherPriority.Background, new Action(delegate { }));
			}

			if (null != t.Exception)
			{
				var innerException = t.Exception.Flatten().InnerException;
				Console.WriteLine("{0}: {1}",
					innerException.GetType(), innerException.Message);
			}
		}

		static void prepareGUIApplication()
		{
			var app = new Application();
			var win = new Window();
			var panel = new StackPanel();
			var button = new Button();
			_label = new Label();
			_label.FontSize = 32;
			_label.Height = 200;
			_label.Content = new TextBlock {Text = "Hello!"};
			button.Height = 100;
			button.FontSize = 32;
			button.Content = new TextBlock
			{
				Text = "Start asynchronous operations"
			};
			button.Click += Click;
			panel.Children.Add(_label);
			panel.Children.Add(button);
			win.Content = panel;
			app.Run(win);
		}
	}
}
