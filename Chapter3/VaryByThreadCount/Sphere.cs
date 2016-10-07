using System;
using System.Drawing;

namespace ConcurrencyBook.Samples
{
	public class Sphere
	{
		private readonly double _x;
		private readonly double _y;
		private readonly double _z;
		private readonly double _radius;
		private readonly Color _color;

		public double X
		{
			get { return _x; }
		}

		public double Y
		{
			get { return _y; }
		}

		public double Z
		{
			get { return _z; }
		}

		public double Radius
		{
			get { return _radius; }
		}

		public Color Color
		{
			get { return _color; }
		}

		public Sphere(
			double x,
			double y,
			double z,
			double radius,
			Color color)
		{
			_x = x;
			_y = y;
			_z = z;
			_radius = radius;
			_color = color;
		}

		public static double GetCoord(
			double i1,
			double i2,
			double w1,
			double w2,
			double p)
		{
			return ((p - i1)/(i2 - i1))*(w2 - w1) + w1;
		}

		public static double modv(double vx, double vy, double vz)
		{
			return Math.Sqrt(vx*vx + vy*vy + vz*vz);
		}

		public static double GetSphereIntersec(double cx, double cy, double cz,
			double radius, double px, double py, double pz,
			double vx, double vy, double vz)
		{
			var A = (vx*vx + vy*vy + vz*vz);
			var B = 2.0*(px*vx + py*vy + pz*vz - vx*cx - vy*
			             cy - vz*cz);
			var C = px*px - 2*px*cx + cx*cx + py*py - 2*py*
			        cy + cy*cy + pz*pz - 2*pz*cz + cz*cz -
			        radius*radius;
			var D = B*B - 4*A*C;
			var t = -1.0;
			if (D >= 0)
			{
				var t1 = (-B - Math.Sqrt(D))/(2.0*A);
				var t2 = (-B + Math.Sqrt(D))/(2.0*A);
				t = t1 > t2 ? t1 : t2;
			}
			return t;
		}

		public static double GetCosAngleV1V2(double v1x, double v1y, double v1z,
			double v2x, double v2y, double v2z)
		{
			return
				(v1x*v2x + v1y*v2y + v1z*v2z)
				/(modv(v1x, v1y, v1z)
				*modv(v2x, v2y, v2z));
		}
	}
}