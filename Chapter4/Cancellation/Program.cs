using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		private static void RunTest(
			Action<CancellationToken> action,
			string name)
		{
			var cancelSource = new CancellationTokenSource();
			var cancelToken = cancelSource.Token;
			var task =
				Task
					.Factory
					.StartNew(() => action(cancelToken), cancelToken);

			// Wait for starting task
			while (task.Status != TaskStatus.Running) { }

			var sw = Stopwatch.StartNew();
			cancelSource.Cancel();
			while (!task.IsCompleted) { }
			sw.Stop();
			Console.WriteLine("{0} task got cancelled in {1} ms", name, sw.ElapsedMilliseconds);
		}

		static void Main()
		{
			RunTest(
				tok =>
				{
					while (true)
					{
						Thread.Sleep(100);
						if (tok.IsCancellationRequested)
							break;
					}
				},
				"CheckFlag");

			RunTest(
				tok =>
				{
					while (true)
					{
						Thread.Sleep(100);
						tok.ThrowIfCancellationRequested();
					}
				},
				"ThrowException");

			RunTest(
				tok =>
				{
					var evt = new ManualResetEvent(false);
					while (true)
					{
						WaitHandle.WaitAny(new[] { evt, tok.WaitHandle }, 100);
						tok.ThrowIfCancellationRequested();
					}
				},
				"WaitHandle");

			const int port = 8083;
			// This expression construct and start tcp server mock
			new Thread(
				() =>
				{
					var listener = new TcpListener(IPAddress.Any, port);
					listener.Start();
					while (true)
						using (var client = listener.AcceptTcpClient())
						using (var stream = client.GetStream())
						using (var writer = new StreamWriter(stream))
						{
							Thread.Sleep(100);
							writer.WriteLine("OK");
						}
				}) {IsBackground = true}
				.Start();

			RunTest(
				tok =>
				{
					while (true)
					{
						using (var client = new TcpClient())
						using (tok.Register(client.Close))
						{
							client.Connect("localhost", port);
							using (var stream = client.GetStream())
							using (var reader = new StreamReader(stream))
								Console.WriteLine(reader.ReadLine());
						}
						tok.ThrowIfCancellationRequested();
					}
				},
				"Callback");
		}
	}
}
