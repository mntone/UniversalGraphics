using Android.Graphics;
using System;
using System.Diagnostics;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGPath : IUGPath
	{
		object IUGObject.Native => Native;
		public Path Native
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(nameof(Native));
				}

				return _native;
			}
		}

		private bool _disposed = false;
		private Path _native;

		public UGPath(IUGContext context)
			=> _native = new Path();

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		public void Reset()
		{
			Debug.Assert(_native != null);
			_native.Dispose();

			_native = new Path();
		}

		public void Close() => Native.Close();

		public void MoveTo(float startX, float startY)
			=> Native.MoveTo(startX, startY);

		public void LineTo(float endX, float endY)
			=> Native.LineTo(endX, endY);

		public void CubicTo(float control1X, float control1Y, float control2X, float control2Y, float endX, float endY)
			=> Native.CubicTo(control1X, control1Y, control2X, control2Y, endX, endY);

		public void QuadTo(float controlX, float controlY, float endX, float endY)
			=> Native.QuadTo(controlX, controlY, endX, endY);

		public void AddCircle(float x, float y, float radius)
			=> Native.AddCircle(x, y, radius, Path.Direction.Cw);

		public void AddCircleInRectangle(float x, float y, float length)
			=> Native.AddOval(x, y, length, length, Path.Direction.Cw);

		public void AddEllipse(float x, float y, float radiusX, float radiusY)
			=> Native.AddOval(x - radiusX, y - radiusY, 2F * radiusX, 2F * radiusY, Path.Direction.Cw);

		public void AddEllipseInRectangle(float x, float y, float width, float height)
			=> Native.AddOval(x, y, width, height, Path.Direction.Cw);

		public void AddRectangle(float x, float y, float width, float height)
			=> Native.AddRect(x, y, width, height, Path.Direction.Cw);

		public void AddRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
			=> Native.AddRoundRect(x, y, width, height, radiusX, radiusY, Path.Direction.Cw);

		public static implicit operator Path(UGPath d)
			=> d.Native;
	}
}
