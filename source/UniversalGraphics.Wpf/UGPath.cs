using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public sealed class UGPath : IUGPath
	{
		object IUGObject.Native => Native;
		public Geometry Native
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
		private PathFigure _figure = null;
		private Geometry _native = null;

		public UGPath(IUGContext context) { }

		public void Dispose()
		{
			Debug.Assert(!_disposed);
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public void Reset()
		{
			Debug.Assert(Native != null);
			_native = null;
		}

		private void CheckPathBuilder()
		{
			if (_figure == null)
			{
				_figure = new PathFigure();
			}
		}

		private void InvalidateGeometry()
		{
			if (_figure != null)
			{
				var geometry = new PathGeometry(new[] { _figure });
				AddGeometry(geometry);

				_figure = null;
			}
		}

		private void AddGeometry(Geometry geometry)
		{
			var native = _native;
			if (native == null)
			{
				_native = geometry;
			}
			else
			{
				if (!(native is PathGeometry pathGeometry))
				{
					pathGeometry = PathGeometry.CreateFromGeometry(native);
					_native = pathGeometry;
				}
				pathGeometry.AddGeometry(geometry);
			}
		}

		public void Close()
		{
			_figure.IsClosed = true;

			InvalidateGeometry();
		}

		public void MoveTo(float startX, float startY)
		{
			CheckPathBuilder();

			_figure.StartPoint = new Point(startX, startY);
		}

		public void LineTo(float endX, float endY)
		{
			CheckPathBuilder();

			_figure.Segments.Add(new LineSegment(new Point(endX, endY), true));
		}

		public void CubicTo(float control1X, float control1Y, float control2X, float control2Y, float endX, float endY)
		{
			CheckPathBuilder();

			_figure.Segments.Add(new BezierSegment(
				new Point(control1X, control1Y),
				new Point(control2X, control2Y),
				new Point(endX, endY),
				true));
		}

		public void QuadTo(float controlX, float controlY, float endX, float endY)
		{
			CheckPathBuilder();

			_figure.Segments.Add(new QuadraticBezierSegment(
				new Point(controlX, controlY),
				new Point(endX, endY),
				true));
		}

		public void AddCircle(float x, float y, float radius)
		{
			InvalidateGeometry();

			var geometry = new EllipseGeometry(new Point(x, y), radius, radius);
			AddGeometry(geometry);
		}

		public void AddCircleInRectangle(float x, float y, float length)
		{
			InvalidateGeometry();

			var radius = length / 2F;
			var geometry = new EllipseGeometry(new Point(x, y), radius, radius);
			AddGeometry(geometry);
		}

		public void AddEllipse(float x, float y, float radiusX, float radiusY)
		{
			InvalidateGeometry();

			var geometry = new EllipseGeometry(new Point(x, y), radiusX, radiusY);
			AddGeometry(geometry);
		}

		public void AddEllipseInRectangle(float x, float y, float width, float height)
		{
			InvalidateGeometry();

			var radiusX = width / 2F;
			var radiusY = height / 2F;
			var geometry = new EllipseGeometry(new Point(x, y), radiusX, radiusY);
			AddGeometry(geometry);
		}

		public void AddRectangle(float x, float y, float width, float height)
		{
			InvalidateGeometry();

			var geometry = new RectangleGeometry(new Rect(x, y, width, height));
			AddGeometry(geometry);
		}

		public void AddRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
		{
			InvalidateGeometry();

			var geometry = new RectangleGeometry(new Rect(x, y, radiusX, radiusY));
			AddGeometry(geometry);
		}

		public static implicit operator Geometry(UGPath d)
			=> d.Native;
	}
}
