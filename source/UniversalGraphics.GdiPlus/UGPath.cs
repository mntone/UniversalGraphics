using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGPath : IUGPath
	{
		object IUGObject.Native => Native;
		public GraphicsPath Native
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

		private readonly GraphicsPath _native;

		private bool _disposed = false;
		private PointF _currentPoint;

		public UGPath(IUGContext context)
		{
			_native = new GraphicsPath();

			_currentPoint = new PointF();
		}

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			_native.Dispose();
			GC.SuppressFinalize(this);
		}

		private void SetCurrentPoint(float x, float y)
		{
			_currentPoint.X = x;
			_currentPoint.Y = y;
		}

		public void Reset() => Native.Reset();

		public void Close() => Native.CloseFigure();

		public void MoveTo(float startX, float startY)
			=> SetCurrentPoint(startX, startY);

		public void LineTo(float endX, float endY)
		{
			Native.AddLine(_currentPoint.X, _currentPoint.Y, endX, endY);
			SetCurrentPoint(endX, endY);
		}

		public void CubicTo(float control1X, float control1Y, float control2X, float control2Y, float endX, float endY)
		{
			Native.AddBezier(_currentPoint.X, _currentPoint.Y, control1X, control1Y, control2X, control2Y, endX, endY);
			SetCurrentPoint(endX, endY);
		}

		public void QuadTo(float controlX, float controlY, float endX, float endY)
		{
			var controlPX = 2F * controlX / 3F;
			var controlPY = 2F * controlY / 3F;
			var control1X = _currentPoint.X / 3F + controlPX;
			var control1Y = _currentPoint.Y / 3F + controlPY;
			var control2X = controlPX + endX / 3F;
			var control2Y = controlPY / 3F + endY / 3F;
			Native.AddBezier(_currentPoint.X, _currentPoint.Y, control1X, control1Y, control2X, control2Y, endX, endY);
			SetCurrentPoint(endX, endY);
		}

		public void AddCircle(float x, float y, float radius)
			=> Native.AddEllipse(x - radius, y - radius, 2F * radius, 2F * radius);

		public void AddCircleInRectangle(float x, float y, float length)
			=> Native.AddEllipse(x, y, length, length);

		public void AddEllipse(float x, float y, float radiusX, float radiusY)
			=> Native.AddEllipse(x - radiusX, y - radiusY, 2F * radiusX, 2F * radiusY);

		public void AddEllipseInRectangle(float x, float y, float width, float height)
			=> Native.AddEllipse(x, y, width, height);

		public void AddRectangle(float x, float y, float width, float height)
			=> Native.AddRectangle(new RectangleF(x, y, width, height));

		public void AddRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
		{
			var right = x + width;
			var bottom = y + height;
			var native = Native;
			native.StartFigure();
			native.AddLine(x + radiusX, y, right - radiusX, y);
			native.AddArc(right - 2F * radiusX, y, 2F * radiusX, 2F * radiusY, -90F, 90F);
			native.AddLine(right, y + radiusY, right, bottom - radiusY);
			native.AddArc(right - 2F * radiusX, bottom - 2F * radiusY, 2F * radiusX, 2F * radiusY, 0F, 90F);
			native.AddLine(right - radiusX, bottom, x + radiusX, bottom);
			native.AddArc(x, bottom - 2F * radiusY, 2F * radiusX, 2F * radiusY, 90F, 90F);
			native.AddLine(x, bottom - radiusY, x, y + radiusY);
			native.AddArc(x, y, 2F * radiusX, 2F * radiusY, -180F, 90F);
			native.CloseFigure();
		}

		public static implicit operator GraphicsPath(UGPath d)
			=> d.Native;
	}
}
