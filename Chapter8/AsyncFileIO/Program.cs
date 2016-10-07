using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsyncFileIO
{
	internal class Program
	{
		private const int BUFFER_SIZE = 4096;

		private static void Main(string[] args)
		{
			var t = ProcessAsynchronousIO();
			t.GetAwaiter().GetResult();
		}

		private static async Task ProcessAsynchronousIO()
		{
			using (var stream = new FileStream("test1.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE))
			{
				Console.WriteLine("1. Uses I/O Threads: {0}", stream.IsAsync);

				var buffer = Encoding.UTF8.GetBytes(CreateFileContent());
				var t = stream.WriteAsync(buffer, 0, buffer.Length);
				await t;
			}

			using (
				var stream = new FileStream("test2.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE,
					FileOptions.Asynchronous))
			{
				Console.WriteLine("2. Uses I/O Threads: {0}", stream.IsAsync);

				var buffer = Encoding.UTF8.GetBytes(CreateFileContent());
				var t = stream.WriteAsync(buffer, 0, buffer.Length);
				await t;
			}

			using (var stream = File.Create("test3.txt", BUFFER_SIZE, FileOptions.Asynchronous))
			using (var sw = new StreamWriter(stream))
			{
				Console.WriteLine("3. Uses I/O Threads: {0}", stream.IsAsync);

				await sw.WriteAsync(CreateFileContent());
			}

			using (var sw = new StreamWriter("test4.txt", append: true))
			{
				Console.WriteLine("4. Uses I/O Threads: {0}", ((FileStream) sw.BaseStream).IsAsync);

				await sw.WriteAsync(CreateFileContent());
			}

			Console.WriteLine("Deleting files...");

			var deleteTasks = new Task[4];
			for (var i = 0; i < 4; i++)
			{
				var fileName = string.Format("test{0}.txt", i + 1);
				deleteTasks[i] = SimulateAsynchronousDelete(fileName);
			}

			await Task.WhenAll(deleteTasks);

			Console.WriteLine("Deleting complete.");
		}

		private static string CreateFileContent()
		{
			var sb = new StringBuilder();
			for (var i = 0; i < 100000; i++)
			{
				sb.AppendFormat("{0}", new Random(i).Next(0, 99999));
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private static Task SimulateAsynchronousDelete(string fileName)
		{
			// No delete async in API
			return Task.Run(() => File.Delete(fileName));
		}
	}
}