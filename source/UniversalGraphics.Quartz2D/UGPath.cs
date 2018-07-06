using CoreGraphics;
using System;
using System.Diagnostics;

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGPath : IUGPath
	{
		object IUGObject.Native => Native;
		public CGPath Native
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
		private CGPath _native;

		public UGPath(IUGContext context)
			=> _native = new CGPath();

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

			_native = new CGPath();
		}

		public void Close() => Native.CloseSubpath();

		public void MoveTo(float startX, float startY)
			=> Native.MoveToPoint(startX, startY);

		public void LineTo(float endX, float endY)
			=> Native.AddLineToPoint(endX, endY);

		public void CubicTo(float control1X, float control1Y, float control2X, float control2Y, float endX, float endY)
			=> Native.AddCurveToPoint(control1X, control1Y, control2X, control2Y, endX, endY);

		public void QuadTo(float controlX, float controlY, float endX, float endY)
			=> Native.AddQuadCurveToPoint(controlX, controlY, endX, endY);

		public void AddCircle(float x, float y, float radius)
			=> Native.AddEllipseInRect(new CGRect(x - radius, y - radius, 2F * radius, 2F * radius));

		public void AddCircleInRectangle(float x, float y, float length)
			=> Native.AddEllipseInRect(new CGRect(x, y, length, length));

		public void AddEllipse(float x, float y, float radiusX, float radiusY)
			=> Native.AddEllipseInRect(new CGRect(x - radiusX, y - radiusY, 2F * radiusX, 2F * radiusY));

		public void AddEllipseInRectangle(float x, float y, float width, float height)
			=> Native.AddEllipseInRect(new CGRect(x, y, width, height));

		public void AddRectangle(float x, float y, float width, float height)
			=> Native.AddRect(new CGRect(x, y, width, height));

		public void AddRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
			=> Native.AddRoundedRect(new CGRect(x, y, width, height), radiusX, radiusY);

		public static implicit operator CGPath(UGPath d)
			=> d.Native;
	}
}
