using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UniversalGraphics.GdiPlus
{
	public sealed class UGContext : IUGContext
	{
		object IUGObject.Native => Native;
		public Graphics Native { get; }
		
		private readonly ColorService _colorService;
		private readonly Action _disposeAction;

		internal UGContext(Graphics native, UGSize canvasRect, float scale, ColorService service)
			: this(native, canvasRect, scale, service, null)
		{ }

		internal UGContext(Graphics native, UGSize canvasRect, float scale, ColorService colorService, Action disposeAction)
		{
			Native = native;
			CanvasSize = canvasRect;
			ScaleFactor = scale;
			_colorService = colorService;
			_disposeAction = disposeAction;

			native.CompositingQuality = CompositingQuality.HighQuality | CompositingQuality.GammaCorrected;
			native.PixelOffsetMode = PixelOffsetMode.Half;
			native.SmoothingMode = SmoothingMode.AntiAlias;
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
			get => Native.SmoothingMode == SmoothingMode.AntiAlias || Native.SmoothingMode == SmoothingMode.HighQuality;
			set => Native.SmoothingMode = value ? SmoothingMode.AntiAlias : SmoothingMode.None;
		}

		public UGSize CanvasSize { get; }
		public float ScaleFactor { get; }
		public int Dpi => (int)(96F * ScaleFactor + .5F);

		public void Flush() { }

		public void ClearColor(UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			Native.Clear(translatedColor);
		}

		public IDisposable CreateLayer()
		{
			var native = Native;
			var state = native.Save();
			return Disposable.Create(() => native.Restore(state));
		}

		public IDisposable CreateLayer(UGRect clipRect)
		{
			var native = Native;
			var state = native.Save();
			native.SetClip(clipRect.ToGDIRect());
			return Disposable.Create(() => native.Restore(state));
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				var pt1 = new Point((int)(startX + 0.5F), (int)(startY + 0.5F));
				var pt2 = new Point((int)(endX + 0.5F), (int)(endY + 0.5F));
				Native.DrawLine(pen, pt1, pt2);
			}
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawLine(pen, startX, startY, endX, endY);
			}
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				Native.DrawEllipse(pen, centerX - radius, centerY - radius, 2F * radius, 2F * radius);
			}
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawEllipse(pen, centerX - radius, centerY - radius, 2F * radius, 2F * radius);
			}
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				Native.DrawEllipse(pen, x, y, length, length);
			}
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawEllipse(pen, x, y, length, length);
			}
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				Native.DrawEllipse(pen, centerX - radiusX, centerY - radiusY, 2F * radiusX, 2F * radiusY);
			}
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawEllipse(pen, centerX - radiusX, centerY - radiusY, 2F * radiusX, 2F * radiusY);
			}
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				Native.DrawEllipse(pen, x, y, width, height);
			}
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawEllipse(pen, x, y, width, height);
			}
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				Native.DrawPath(pen, ((UGPath)path).Native);
			}
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawPath(pen, ((UGPath)path).Native);
			}
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				GraphicsState state = null;
				try
				{
					state = Native.Save();
					Native.TranslateTransform(x, y);
					Native.DrawPath(pen, ((UGPath)path).Native);
				}
				finally { Native.Restore(state); }
			}
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);

				GraphicsState state = null;
				try
				{
					state = Native.Save();
					Native.TranslateTransform(x, y);
					Native.DrawPath(pen, ((UGPath)path).Native);
				}
				finally { Native.Restore(state); }
			}
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				Native.DrawRectangle(pen, x, y, width, height);
			}
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawRectangle(pen, x, y, width, height);
			}
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			using (var path = Native.PrepareRoundRectPath(x, y, width, height, radiusX, radiusY))
			{
				Native.DrawPath(pen, path);
			}
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var pen = new Pen(translatedColor, strokeWidth))
			using (var path = Native.PrepareRoundRectPath(x, y, width, height, radiusX, radiusY))
			{
				pen.SetStrokeStyle(strokeStyle);
				Native.DrawPath(pen, path);
			}
		}

		public void DrawImage(IUGCanvasImage image, float x, float y)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				Native.DrawImage(renderTarget.Native, x, y);
			}
		}

		public void DrawImage(IUGCanvasImage image, float x, float y, float width, float height)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				Native.DrawImage(renderTarget.Native, x, y, width, height);
			}
		}

		public void FillCircle(float centerX, float centerY, float radius, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			{
				Native.FillEllipse(brush, centerX - radius, centerY - radius, 2F * radius, 2F * radius);
			}
		}

		public void FillCircle(float centerX, float centerY, float radius, IUGBrush brush)
		{
			var x = centerX - radius;
			var y = centerY - radius;
			var native = Native;
			GraphicsState state = null;
			try
			{
				state = native.Save();
				native.TranslateTransform(x, y);
				native.ScaleTransform(radius, radius);
				var nativeBrush = ((IUGBrushInternal)brush).Native;
				Native.FillEllipse(nativeBrush, 0F, 0F, 1F, 1F);
			}
			finally { native.Restore(state); }
		}

		public void FillCircleInRectangle(float x, float y, float length, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			{
				Native.FillEllipse(brush, x, y, length, length);
			}
		}

		public void FillCircleInRectangle(float x, float y, float length, IUGBrush brush)
		{
			var native = Native;
			GraphicsState state = null;
			try
			{
				state = native.Save();
				native.TranslateTransform(x, y);
				native.ScaleTransform(length, length);
				var nativeBrush = ((IUGBrushInternal)brush).Native;
				Native.FillEllipse(nativeBrush, 0F, 0F, 1F, 1F);
			}
			finally { native.Restore(state); }
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			{
				Native.FillEllipse(brush, centerX - radiusX, centerY - radiusY, 2F * radiusX, 2F * radiusY);
			}
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, IUGBrush brush)
		{
			var x = centerX - radiusX;
			var y = centerY - radiusY;
			var native = Native;
			GraphicsState state = null;
			try
			{
				state = native.Save();
				native.TranslateTransform(x, y);
				native.ScaleTransform(radiusX, radiusY);
				var nativeBrush = ((IUGBrushInternal)brush).Native;
				Native.FillEllipse(nativeBrush, 0F, 0F, 1F, 1F);
			}
			finally { native.Restore(state); }
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			{
				Native.FillEllipse(brush, x, y, width, height);
			}
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			var native = Native;
			GraphicsState state = null;
			try
			{
				state = native.Save();
				native.TranslateTransform(x, y);
				native.ScaleTransform(width, height);
				var nativeBrush = ((IUGBrushInternal)brush).Native;
				Native.FillEllipse(nativeBrush, 0F, 0F, 1F, 1F);
			}
			finally { native.Restore(state); }
		}

		public void FillPath(IUGPath path, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			{
				Native.FillPath(brush, ((UGPath)path).Native);
			}
		}

		public void FillPath(IUGPath path, float x, float y, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			{
				GraphicsState state = null;
				try
				{
					state = Native.Save();
					Native.TranslateTransform(x, y);
					Native.FillPath(brush, ((UGPath)path).Native);
				}
				finally { Native.Restore(state); }
			}
		}

		public void FillRectangle(float x, float y, float width, float height, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			{
				Native.FillRectangle(brush, x, y, width, height);
			}
		}

		public void FillRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			var native = Native;
			GraphicsState state = null;
			try
			{
				state = native.Save();
				native.TranslateTransform(x, y);
				native.ScaleTransform(width, height);
				var nativeBrush = ((IUGBrushInternal)brush).Native;
				native.FillRectangle(nativeBrush, 0F, 0F, 1F, 1F);
			}
			finally { native.Restore(state); }
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color)
		{
			var translatedColor = _colorService.GetTranslatedColor(color);
			using (var brush = new SolidBrush(translatedColor))
			using (var path = Native.PrepareRoundRectPath(x, y, width, height, radiusX, radiusY))
			{
				Native.FillPath(brush, path);
			}
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, IUGBrush brush)
		{
			var native = Native;
			GraphicsState state = null;
			try
			{
				state = native.Save();
				native.TranslateTransform(x, y);
				native.ScaleTransform(width, height);
				var nativeBrush = ((IUGBrushInternal)brush).Native;
				using (var path = Native.PrepareRoundRectPath(0F, 0F, 1F, 1F, radiusX / width, radiusY / height))
				{
					native.FillPath(nativeBrush, path);
				}
			}
			finally { native.Restore(state); }
		}

		public void Rotate(float degrees) => Native.RotateTransform(degrees);
		public void Rotate(float degrees, float centerX, float centerY)
		{
			var radians = MathHelper.DegreesToRadians(degrees);
			Native.MultiplyTransform(MatrixHelper.CreateRotation(radians, centerX, centerY));
		}

		public void RotateRadians(float radians)
		{
			var degrees = MathHelper.RadiansToDegrees(radians);
			Native.RotateTransform(degrees);
		}

		public void RotateRadians(float radians, float centerX, float centerY)
			=> Native.MultiplyTransform(MatrixHelper.CreateRotation(radians, centerX, centerY));

		public void Scale(float scaleX, float scaleY) => Native.ScaleTransform(scaleX, scaleY);
		public void Scale(float scaleX, float scaleY, float centerX, float centerY)
			=> Native.MultiplyTransform(MatrixHelper.CreateScale(scaleX, scaleY, centerX, centerY));

		public void Skew(float degreesX, float degreesY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
			Native.MultiplyTransform(MatrixHelper.CreateSkew(radiansX, radiansY));
		}

		public void Skew(float degreesX, float degreesY, float centerX, float centerY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
			Native.MultiplyTransform(MatrixHelper.CreateSkew(radiansX, radiansY, centerX, centerY));
		}

		public void SkewRadians(float radiansX, float radiansY)
			=> Native.MultiplyTransform(MatrixHelper.CreateSkew(radiansX, radiansY));

		public void SkewRadians(float radiansX, float radiansY, float centerX, float centerY)
			=> Native.MultiplyTransform(MatrixHelper.CreateSkew(radiansX, radiansY, centerX, centerY));

		public void Transform(Matrix3x2 transformMatrix)
			=> Native.MultiplyTransform(new Matrix(
				transformMatrix.M11, transformMatrix.M12,
				transformMatrix.M21, transformMatrix.M22,
				transformMatrix.M31, transformMatrix.M32));

		public void Translate(float translateX, float translateY) => Native.TranslateTransform(translateX, translateY);

		#endregion

		public static implicit operator Graphics(UGContext d)
			=> d.Native;
	}

	internal static class GdiPlusExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GraphicsPath PrepareRoundRectPath(this Graphics native, float x, float y, float width, float height, float radiusX, float radiusY)
		{
			var right = x + width;
			var bottom = y + height;
			var path = new GraphicsPath();
			path.StartFigure();
			path.AddLine(x + radiusX, y, right - radiusX, y);
			path.AddArc(right - 2F * radiusX, y, 2F * radiusX, 2F * radiusY, -90F, 90F);
			path.AddLine(right, y + radiusY, right, bottom - radiusY);
			path.AddArc(right - 2F * radiusX, bottom - 2F * radiusY, 2F * radiusX, 2F * radiusY, 0F, 90F);
			path.AddLine(right - radiusX, bottom, x + radiusX, bottom);
			path.AddArc(x, bottom - 2F * radiusY, 2F * radiusX, 2F * radiusY, 90F, 90F);
			path.AddLine(x, bottom - radiusY, x, y + radiusY);
			path.AddArc(x, y, 2F * radiusX, 2F * radiusY, -180F, 90F);
			path.CloseFigure();
			return path;
		}
	}
}
