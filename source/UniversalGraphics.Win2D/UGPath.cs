using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Diagnostics;
using Microsoft.Graphics.Canvas;

#if WINDOWS_APP || WINDOWS_PHONE_APP
using Microsoft.Graphics.Canvas.Numerics;
#else
using System.Numerics;
#endif

namespace UniversalGraphics.Win2D
{
	public sealed class UGPath : IUGPath
	{
		object IUGObject.Native => Native;
		public CanvasGeometry Native
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

		private readonly UGContext _context;

		private CanvasDevice Device => _context.Device;

		private bool _disposed = false;
		private CanvasPathBuilder _builder = null;
		private CanvasGeometry _native;

		public UGPath(IUGContext context)
		{
			_context = (UGContext)context;
		}

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
			var native = _native;
			if (native != null)
			{
				native.Dispose();
				_native = null;
			}
			_builder?.Dispose();
		}

		private void CheckPathBuilder()
		{
			if (_builder == null)
			{
				_builder = new CanvasPathBuilder(Device);
			}
		}

		private void InvalidateGeometry()
		{
			if (_builder != null)
			{
				var geometry = CanvasGeometry.CreatePath(_builder);
				AddGeometry(geometry);

				_builder = null;
			}
		}

		private void AddGeometry(CanvasGeometry geometry)
		{
			var native = _native;
			if (native == null)
			{
				_native = geometry;
			}
			else
			{
#if WINDOWS_APP || WINDOWS_PHONE_APP
				_native = native.CombineWith(geometry, new Matrix3x2() { M11 = 1F, M22 = 1F }, CanvasGeometryCombine.Union);
#else
				_native = native.CombineWith(geometry, Matrix3x2.Identity, CanvasGeometryCombine.Union);
#endif
			}
		}

		public void Close()
		{
			_builder.EndFigure(CanvasFigureLoop.Closed);

			InvalidateGeometry();
		}

		public void MoveTo(float startX, float startY)
		{
			CheckPathBuilder();

			_builder.BeginFigure(startX, startY);
		}

		public void LineTo(float endX, float endY)
		{
			CheckPathBuilder();

			_builder.AddLine(endX, endY);
		}

		public void CubicTo(float control1X, float control1Y, float control2X, float control2Y, float endX, float endY)
		{
			CheckPathBuilder();

			_builder.AddCubicBezier(new Vector2() { X = control1X, Y = control1Y }, new Vector2() { X = control2X, Y = control2Y }, new Vector2() { X = endX, Y = endY });
		}

		public void QuadTo(float controlX, float controlY, float endX, float endY)
		{
			CheckPathBuilder();

			_builder.AddQuadraticBezier(new Vector2() { X = controlX, Y = controlY }, new Vector2() { X = endX, Y = endY });
		}

		public void AddCircle(float x, float y, float radius)
		{
			InvalidateGeometry();

			var geometry = CanvasGeometry.CreateCircle(Device, x, y, radius);
			AddGeometry(geometry);
		}

		public void AddCircleInRectangle(float x, float y, float length)
		{
			InvalidateGeometry();

			var radius = length / 2F;
			var geometry = CanvasGeometry.CreateEllipse(Device, x - radius, y - radius, radius, radius);
			AddGeometry(geometry);
		}

		public void AddEllipse(float x, float y, float radiusX, float radiusY)
		{
			InvalidateGeometry();

			var geometry = CanvasGeometry.CreateEllipse(Device, x, y, radiusX, radiusY);
			AddGeometry(geometry);
		}

		public void AddEllipseInRectangle(float x, float y, float width, float height)
		{
			InvalidateGeometry();

			var radiusX = width / 2F;
			var radiusY = height / 2F;
			var geometry = CanvasGeometry.CreateEllipse(Device, x - radiusX, y - radiusY, radiusX, radiusY);
			AddGeometry(geometry);
		}

		public void AddRectangle(float x, float y, float width, float height)
		{
			InvalidateGeometry();

			var geometry = CanvasGeometry.CreateRectangle(Device, x, y, width, height);
			AddGeometry(geometry);
		}

		public void AddRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
		{
			InvalidateGeometry();

			var geometry = CanvasGeometry.CreateRoundedRectangle(Device, x, y, width, height, radiusX, radiusY);
			AddGeometry(geometry);
		}

		public static implicit operator CanvasGeometry(UGPath d)
			=> d.Native;
	}
}
