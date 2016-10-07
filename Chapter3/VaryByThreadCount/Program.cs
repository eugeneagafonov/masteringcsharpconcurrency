using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConcurrencyBook.Samples
{
	/// <summary>
	/// Vary by thread count sample
	/// </summary>
	/// <remarks>
	/// Raytracing code based on http://www.codeproject.com/Articles/19732/Simple-Ray-Tracing-in-C
	/// </remarks>
	internal static class Program
	{
		private const int _width = 1920;
		private const int _height = 1920;
		private const double _viewerX = 0;
		private const double _viewerY = 0;
		private const double _viewerZ = 500;
		private const double _lightX = 200;
		private const double _lightY = 200;
		private const double _lightZ = 200;
		private const double _lightVectorX = -1;
		private const double _lightVectorY = -1;
		private const double _lightVectorZ = -1;

		private static void RenderScene(Color[,] data, int startCol, int endCol)
		{
			var objects =
				new List<Sphere>
				{
					new Sphere(0.0, 0.0, 90.0, 100.0, Color.Lavender),
					new Sphere(-180.0, -130.0, -110.0, 15.0, Color.Red),
					new Sphere(-140.0, -140.0, -150.0, 20.0, Color.Orange),
					new Sphere(-40.0, -30.0, 180.0, 30.0, Color.LightGreen),
				};
			const double fMax = 200.0;
			for (var i = startCol; i <= endCol; i++)
			{
				var x = Sphere.GetCoord(0, _width, -fMax, fMax, i);
				for (var j = 0; j < _height; j++)
				{
					var y = Sphere.GetCoord(0, _height, fMax, -fMax, j);
					var t = 1.0E10;
					double vx = x - _viewerX, vy = y - _viewerY, vz = -_viewerZ;
					var modV = Sphere.modv(vx, vy, vz);
					vx = vx/modV;
					vy = vy/modV;
					vz = vz/modV;
					Sphere spherehit = null;
					foreach (var sphn in objects)
					{
						var taux = Sphere.GetSphereIntersec(sphn.X, sphn.Y,
							sphn.Z,
							sphn.Radius, _viewerX, _viewerY, _viewerZ, vx, vy, vz);
						if (taux < 0) continue;
						if (taux > 0 && taux < t)
						{
							t = taux;
							spherehit = sphn;
						}
					}
					var color = Color.FromArgb(10, 20, 10);
					if (spherehit != null)
					{
						double itx = _viewerX + t*vx,
							ity = _viewerY + t*vy,
							itz = _viewerZ +
							      t*vz;
						// shadow
						var tauxla = Sphere.GetSphereIntersec(spherehit.X,
							spherehit.Y, spherehit.Z, spherehit.Radius,
							_lightX, _lightY, _lightZ, itx - _lightX,
							ity - _lightY, itz - _lightZ);

						foreach (Sphere t1 in objects)
						{
							var sphnb = t1;
							if (sphnb != spherehit)
							{
								var tauxlb = Sphere.GetSphereIntersec(sphnb.X,
									sphnb.Y, sphnb.Z, sphnb.Radius, _lightX,
									_lightY, _lightZ, itx - _lightX, ity - _lightY, itz -
									                                                _lightZ);
								if (tauxlb > 0 && tauxla < tauxlb)
								{
									break;
								}
							}
						}
						var cost =
							Sphere.GetCosAngleV1V2(
								_lightVectorX,
								_lightVectorY,
								_lightVectorZ,
								itx - spherehit.X,
								ity - spherehit.Y,
								itz - spherehit.Z);
						if (cost < 0)
							cost = 0;
						const double fact = 1.0;
						var rgbR = spherehit.Color.R*cost*fact;
						var rgbG = spherehit.Color.G*cost*fact;
						var rgbB = spherehit.Color.B*cost*fact;
						color = Color.FromArgb((int) rgbR, (int) rgbG, (int) rgbB);
					}
					data[i, j] = color;
				}
			}
		}

		private static void ShowResult(Color[,] data)
		{
			var bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
			for (var i = 0; i < _width; i++)
				for (var j = 0; j < _height; j++)
					bitmap.SetPixel(i, j, data[i, j]);
			var pic =
				new PictureBox
				{
					Image = bitmap,
					Dock = DockStyle.Fill
				};
			var form =
				new Form
				{
					ClientSize = new Size(_width, _height)
				};
			form.KeyDown += (s, a) => form.Close();
			form.Controls.Add(pic);
			Application.Run(form);
		}

		private static void Main()
		{
			var data = new Color[_width, _height];

			// Warm up
			RenderScene(data, 0, 1);

			// Coarse grained variant
			for (var threadCnt = 1; threadCnt <= 32; threadCnt++)
			{
				var part = _width / threadCnt;
				var threads =
					Enumerable
						.Range(0, threadCnt)
						.Select(
							n =>
							{
								var startCol = n * part;
								var endCol =
									n == threadCnt - 1
										? _width - (threadCnt - 1) * part - 1
										: (n + 1) * part;
								return new Thread(() => RenderScene(data, startCol, endCol));
							})
						.ToArray();
				var sw = Stopwatch.StartNew();
				foreach (var thread in threads)
					thread.Start();
				foreach (var thread in threads)
					thread.Join();
				sw.Stop();
				Console.WriteLine("{0} threads. Render time {1}ms", threadCnt, sw.ElapsedMilliseconds);
			}

			// Fine grained variant
			var tasks = new List<Task>();
			var fineSw = Stopwatch.StartNew();
			for (var i = 0; i < _width; i++)
			{
				var col = i; // Create separate variable for closure
				tasks.Add(Task.Factory.StartNew(() => RenderScene(data, col, col)));
			}
			Task.WaitAll(tasks.ToArray());
			fineSw.Stop();
			Console.WriteLine("Fine grained {0}ms", fineSw.ElapsedMilliseconds);

			ShowResult(data);
		}
	}
}
