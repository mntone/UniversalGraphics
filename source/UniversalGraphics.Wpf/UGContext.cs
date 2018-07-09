using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace UniversalGraphics.Wpf
{
	public sealed class UGContext : IUGContext2
	{
		object IUGObject.Native => Native;
		public DrawingContext Native { get; }

		private readonly Visual _canvas;
		private readonly Size _canvasSize;
		internal readonly Stack<CanvasLayer> _layers;
		private readonly Action _disposeAction;

		internal UGContext(Visual canvas, DrawingContext native, Size canvasSize, float scale)
			: this(canvas, native, canvasSize, scale, null)
		{ }

		internal UGContext(Visual canvas, DrawingContext native, Size canvasSize, float scale, Action disposeAction)
		{
			_canvas = canvas;
			Native = native;
			_canvasSize = canvasSize;
			ScaleFactor = scale;
			_disposeAction = disposeAction;

			_layers = new Stack<CanvasLayer>();
		}

		public void Dispose()
		{
			_disposeAction?.Invoke();
			GC.SuppressFinalize(this);
		}

		#region IUGContext Methods

		IUGFactory IUGContext.Factory => new UGFactory(this);

		public bool Antialiasing
		{
			get => RenderOptions.GetEdgeMode(_canvas) == EdgeMode.Unspecified;
			set => RenderOptions.SetEdgeMode(_canvas, value ? EdgeMode.Unspecified : EdgeMode.Aliased);
		}

		public UGTextAntialiasing TextAntialiasing
		{
			get => TextOptions.GetTextRenderingMode(_canvas).ToUGTextAntialiasing();
			set => TextOptions.SetTextRenderingMode(_canvas, value.ToWPFTextRenderingMode());
		}

		public UGSize CanvasSize => new UGSize((float)_canvasSize.Width, (float)_canvasSize.Height);
		public float ScaleFactor { get; }
		public int Dpi => (int)(96F * ScaleFactor + .5F);

		public void Flush() { }

		public void ClearColor(UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			Native.DrawRectangle(brush, null, new Rect(_canvasSize));
		}

		public IDisposable CreateLayer()
		{
			var layer = new CanvasLayer(this);
			_layers.Push(layer);
			return layer;
		}

		public IDisposable CreateLayer(float alpha)
		{
			var layer = new CanvasLayer(this, alpha);
			_layers.Push(layer);
			return layer;
		}

		public IDisposable CreateLayer(UGRect clipRect)
		{
			var layer = new CanvasLayer(this, clipRect);
			_layers.Push(layer);
			return layer;
		}

		public IDisposable CreateLayer(float alpha, UGRect clipRect)
		{
			var layer = new CanvasLayer(this, alpha, clipRect);
			_layers.Push(layer);
			return layer;
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			var point1 = new Point(startX, startY);
			var point2 = new Point(endX, endY);
			Native.DrawLine(pen, point1, point2);
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			var point1 = new Point(startX, startY);
			var point2 = new Point(endX, endY);
			Native.DrawLine(pen, point1, point2);
		}

		public void DrawLines(IEnumerable<Vector2> points, UGColor color, float strokeWidth)
		{
			var count = points.Count();
			if (count < 2)
			{
				throw new ArgumentException(nameof(points));
			}

			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);

			var native = Native;
			var current = points.First();
			var currentPoint = new Point(current.X, current.Y);
			foreach (var next in points.Skip(1))
			{
				var nextPoint = new Point(next.X, next.Y);
				native.DrawLine(pen, currentPoint, nextPoint);
				currentPoint = nextPoint;
			}
		}

		public void DrawLines(IEnumerable<Vector2> points, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var count = points.Count();
			if (count < 2)
			{
				throw new ArgumentException(nameof(points));
			}

			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);

			var native = Native;
			var current = points.First();
			var currentPoint = new Point(current.X, current.Y);
			foreach (var next in points.Skip(1))
			{
				var nextPoint = new Point(next.X, next.Y);
				native.DrawLine(pen, currentPoint, nextPoint);
				currentPoint = nextPoint;
			}
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			var center = new Point(centerX, centerY);
			Native.DrawEllipse(null, pen, center, radius, radius);
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			var center = new Point(centerX, centerY);
			Native.DrawEllipse(null, pen, center, radius, radius);
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth)
		{
			var radius = length / 2F;
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			var center = new Point(x + radius, y + radius);
			Native.DrawEllipse(null, pen, center, radius, radius);
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var radius = length / 2F;
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			var center = new Point(x + radius, y + radius);
			Native.DrawEllipse(null, pen, center, radius, radius);
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			var center = new Point(centerX, centerY);
			Native.DrawEllipse(null, pen, center, radiusX, radiusY);
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			var center = new Point(centerX, centerY);
			Native.DrawEllipse(null, pen, center, radiusX, radiusY);
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			var radiusX = width / 2F;
			var radiusY = height / 2F;
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			var center = new Point(x + radiusX, y + radiusY);
			Native.DrawEllipse(null, pen, center, radiusX, radiusY);
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var radiusX = width / 2F;
			var radiusY = height / 2F;
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			var center = new Point(x + radiusX, y + radiusY);
			Native.DrawEllipse(null, pen, center, radiusX, radiusY);
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			Native.DrawGeometry(null, pen, ((UGPath)path).Native);
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			Native.DrawGeometry(null, pen, ((UGPath)path).Native);
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			try
			{
				Native.PushTransform(new TranslateTransform(x, y));
				Native.DrawGeometry(null, pen, ((UGPath)path).Native);
			}
			finally { Native.Pop(); }
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			try
			{
				Native.PushTransform(new TranslateTransform(x, y));
				Native.DrawGeometry(null, pen, ((UGPath)path).Native);
			}
			finally { Native.Pop(); }
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			var rect = new Rect(x, y, width, height);
			Native.DrawRectangle(null, pen, rect);
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			var rect = new Rect(x, y, width, height);
			Native.DrawRectangle(null, pen, rect);
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			var rect = new Rect(x, y, width, height);
			Native.DrawRoundedRectangle(null, pen, rect, radiusX, radiusY);
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pen = new Pen(brush, strokeWidth);
			pen.SetStrokeStyle(strokeStyle);
			var rect = new Rect(x, y, width, height);
			Native.DrawRoundedRectangle(null, pen, rect, radiusX, radiusY);
		}

		public void DrawTextLayout(IUGTextLayout textLayout, float x, float y, UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var pTextLayout = (UGTextLayout)textLayout;
			var bounds = pTextLayout.LayoutBounds;
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x + bounds.X, y + bounds.Y));
				native.DrawGlyphRun(brush, pTextLayout.Native);
			}
			finally { native.Pop(); }
		}

		public void DrawImage(IUGCanvasImage image, float x, float y)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				Native.DrawImage(renderTarget.Native, new Rect(x, y, renderTarget.Width, renderTarget.Height));
			}
		}

		public void DrawImage(IUGCanvasImage image, float x, float y, float width, float height)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				Native.DrawImage(renderTarget.Native, new Rect(x, y, width, height));
			}
		}

		public void FillCircle(float centerX, float centerY, float radius, UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var center = new Point(centerX, centerY);
			Native.DrawEllipse(brush, null, center, radius, radius);
		}

		public void FillCircle(float centerX, float centerY, float radius, IUGBrush brush)
		{
			var x = centerX - radius;
			var y = centerY - radius;
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x, y));
				try
				{
					native.PushTransform(new ScaleTransform(radius, radius));

					var nativeBrush = ((IUGBrushInternal)brush).Native;
					native.DrawEllipse(nativeBrush, null, new Point(.5, .5), .5, .5);
				}
				finally { native.Pop(); }
			}
			finally { native.Pop(); }
		}

		public void FillCircleInRectangle(float x, float y, float length, UGColor color)
		{
			var radius = length / 2F;
			var brush = new SolidColorBrush(color.ToWPFColor());
			var center = new Point(x + radius, y + radius);
			Native.DrawEllipse(brush, null, center, radius, radius);
		}

		public void FillCircleInRectangle(float x, float y, float length, IUGBrush brush)
		{
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x, y));
				try
				{
					native.PushTransform(new ScaleTransform(length, length));

					var nativeBrush = ((IUGBrushInternal)brush).Native;
					native.DrawEllipse(nativeBrush, null, new Point(.5, .5), .5, .5);
				}
				finally { native.Pop(); }
			}
			finally { native.Pop(); }
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var center = new Point(centerX, centerY);
			Native.DrawEllipse(brush, null, center, radiusX, radiusY);
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, IUGBrush brush)
		{
			var x = centerX - radiusX;
			var y = centerY - radiusY;
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x, y));
				try
				{
					native.PushTransform(new ScaleTransform(radiusX, radiusY));

					var nativeBrush = ((IUGBrushInternal)brush).Native;
					native.DrawEllipse(nativeBrush, null, new Point(.5, .5), .5, .5);
				}
				finally { native.Pop(); }
			}
			finally { native.Pop(); }
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, UGColor color)
		{
			var radiusX = width / 2F;
			var radiusY = height / 2F;
			var brush = new SolidColorBrush(color.ToWPFColor());
			var center = new Point(x + radiusX, y + radiusY);
			Native.DrawEllipse(brush, null, center, radiusX, radiusY);
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x, y));
				try
				{
					native.PushTransform(new ScaleTransform(width, height));

					var nativeBrush = ((IUGBrushInternal)brush).Native;
					native.DrawEllipse(nativeBrush, null, new Point(.5, .5), .5, .5);
				}
				finally { native.Pop(); }
			}
			finally { native.Pop(); }
		}

		public void FillPath(IUGPath path, UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			Native.DrawGeometry(brush, null, ((UGPath)path).Native);
		}

		public void FillPath(IUGPath path, float x, float y, UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x, y));
				native.DrawGeometry(brush, null, ((UGPath)path).Native);
			}
			finally { native.Pop(); }
		}

		public void FillRectangle(float x, float y, float width, float height, UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var rect = new Rect(x, y, width, height);
			Native.DrawRectangle(brush, null, rect);
		}

		public void FillRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x, y));
				try
				{
					native.PushTransform(new ScaleTransform(width, height));

					var nativeBrush = ((IUGBrushInternal)brush).Native;
					native.DrawRectangle(nativeBrush, null, new Rect(0.0, 0.0, 1.0, 1.0));
				}
				finally { native.Pop(); }
			}
			finally { native.Pop(); }
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color)
		{
			var brush = new SolidColorBrush(color.ToWPFColor());
			var rect = new Rect(x, y, width, height);
			Native.DrawRoundedRectangle(brush, null, rect, radiusX, radiusY);
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, IUGBrush brush)
		{
			var native = Native;
			try
			{
				native.PushTransform(new TranslateTransform(x, y));
				try
				{
					native.PushTransform(new ScaleTransform(width, height));

					var nativeBrush = ((IUGBrushInternal)brush).Native;
					native.DrawRoundedRectangle(nativeBrush, null, new Rect(0.0, 0.0, 1.0, 1.0), radiusX / width, radiusY / height);
				}
				finally { native.Pop(); }
			}
			finally { native.Pop(); }
		}

		public void Rotate(float degrees)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new RotateTransform(degrees));
		}

		public void Rotate(float degrees, float centerX, float centerY)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new RotateTransform(degrees, centerX, centerY));
		}

		public void RotateRadians(float radians)
		{
			var degrees = MathHelper.RadiansToDegrees(radians);

			++_layers.Peek()._count;
			Native.PushTransform(new RotateTransform(degrees));
		}

		public void RotateRadians(float radians, float centerX, float centerY)
		{
			var degrees = MathHelper.RadiansToDegrees(radians);

			++_layers.Peek()._count;
			Native.PushTransform(new RotateTransform(degrees, centerX, centerY));
		}

		public void Scale(float scaleX, float scaleY)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new ScaleTransform(scaleX, scaleY));
		}

		public void Scale(float scaleX, float scaleY, float centerX, float centerY)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new ScaleTransform(scaleX, scaleY, centerX, centerY));
		}

		public void Skew(float degreesX, float degreesY)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new SkewTransform(degreesX, degreesY));
		}

		public void Skew(float degreesX, float degreesY, float centerX, float centerY)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new SkewTransform(degreesX, degreesY, centerX, centerY));
		}

		public void SkewRadians(float radiansX, float radiansY)
		{
			var degreesX = MathHelper.RadiansToDegrees(radiansX);
			var degreesY = MathHelper.RadiansToDegrees(radiansY);

			++_layers.Peek()._count;
			Native.PushTransform(new SkewTransform(degreesX, degreesY));
		}

		public void SkewRadians(float radiansX, float radiansY, float centerX, float centerY)
		{
			var degreesX = MathHelper.RadiansToDegrees(radiansX);
			var degreesY = MathHelper.RadiansToDegrees(radiansY);

			++_layers.Peek()._count;
			Native.PushTransform(new SkewTransform(degreesX, degreesY, centerX, centerY));
		}

		public void Transform(Matrix3x2 transformMatrix)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new MatrixTransform(
				transformMatrix.M11, transformMatrix.M12,
				transformMatrix.M21, transformMatrix.M22,
				transformMatrix.M31, transformMatrix.M32));
		}

		public void Translate(float translateX, float translateY)
		{
			++_layers.Peek()._count;
			Native.PushTransform(new TranslateTransform(translateX, translateY));
		}

		#endregion

		public static implicit operator DrawingContext(UGContext d)
			=> d.Native;
	}
}
