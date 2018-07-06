using CoreGraphics;
using System;
using System.Numerics;

namespace UniversalGraphics.Quartz2D
{
	public sealed class UGContext : IUGContext2
	{
		object IUGObject.Native => Native;
		public CGContext Native { get; }

		private readonly CGRect _canvasRect;
		private readonly Action _disposeAction;

		internal UGContext(CGContext native, CGRect canvasRect, float scale)
			: this(native, canvasRect, scale, null)
		{ }

		internal UGContext(CGContext native, CGRect canvasRect, float scale, Action disposeAction)
		{
			Native = native;
			_canvasRect = canvasRect;
			ScaleFactor = scale;
			_disposeAction = disposeAction;
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
			get => throw new NotSupportedException();
			set => Native.SetAllowsAntialiasing(value);
		}

		public UGSize CanvasSize => new UGSize((float)_canvasRect.Width, (float)_canvasRect.Height);
		public float ScaleFactor { get; }
		public int Dpi => (int)(72F * ScaleFactor + .5F);

		public void Flush() => Native.Flush();

		public void ClearColor(UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.FillRect(_canvasRect);
				}
				finally { native.RestoreState(); }
			}
		}

		public IDisposable CreateLayer()
		{
			var native = Native;
			native.SaveState();
			return Disposable.Create(() => native.RestoreState());
		}

		public IDisposable CreateLayer(float alpha)
		{
			var native = Native;
			native.SaveState();
			native.SetAlpha(alpha);
			return Disposable.Create(() => native.RestoreState());
		}

		public IDisposable CreateLayer(UGRect clipRect)
		{
			var native = Native;
			native.SaveState();
			native.ClipToRect(clipRect.ToCGRect());
			return Disposable.Create(() => native.RestoreState());
		}

		public IDisposable CreateLayer(float alpha, UGRect clipRect)
		{
			var native = Native;
			native.SaveState();
			native.SetAlpha(alpha);
			native.ClipToRect(clipRect.ToCGRect());
			return Disposable.Create(() => native.RestoreState());
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.StrokeLineSegments(new[] { new CGPoint(startX, startY), new CGPoint(endX, endY) });
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.StrokeLineSegments(new[] { new CGPoint(startX, startY), new CGPoint(endX, endY) });
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.StrokeEllipseInRect(new CGRect(centerX - radius, centerY - radius, 2F * radius, 2F * radius));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.StrokeEllipseInRect(new CGRect(centerX - radius, centerY - radius, 2F * radius, 2F * radius));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.StrokeEllipseInRect(new CGRect(x, y, length, length));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.StrokeEllipseInRect(new CGRect(x, y, length, length));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.StrokeEllipseInRect(new CGRect(centerX - radiusX, centerY - radiusY, 2F * radiusX, 2F * radiusY));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.StrokeEllipseInRect(new CGRect(centerX - radiusX, centerY - radiusY, 2F * radiusX, 2F * radiusY));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.StrokeEllipseInRect(new CGRect(x, y, width, height));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.StrokeEllipseInRect(new CGRect(x, y, width, height));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.AddPath(((UGPath)path).Native);
					native.StrokePath();
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.AddPath(((UGPath)path).Native);
					native.StrokePath();
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.TranslateCTM(x, y);
					native.AddPath(((UGPath)path).Native);
					native.StrokePath();
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.TranslateCTM(x, y);
					native.AddPath(((UGPath)path).Native);
					native.StrokePath();
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.StrokeRect(new CGRect(x, y, width, height));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					native.StrokeRect(new CGRect(x, y, width, height));
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					using (var path = CGPath.FromRoundedRect(new CGRect(x, y, width, height), radiusX, radiusY))
					{
						native.AddPath(path);
						native.StrokePath();
					}
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetStrokeColor(nativeColor);
					native.SetLineWidth(strokeWidth);
					native.SetStrokeStyle(strokeWidth, strokeStyle);
					using (var path = CGPath.FromRoundedRect(new CGRect(x, y, width, height), radiusX, radiusY))
					{
						native.AddPath(path);
						native.StrokePath();
					}
				}
				finally { native.RestoreState(); }
			}
		}

		public void DrawImage(IUGCanvasImage image, float x, float y)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				if (renderTarget?.Native != null)
				{
					var cgImage = renderTarget.Native;
					var scaledWidth = (float)cgImage.Width / ScaleFactor;
					var scaledHeight = (float)cgImage.Height / ScaleFactor;
					var destRect = new CGRect(x, y, scaledWidth, scaledHeight);
					Native.DrawImage(destRect, cgImage);
				}
			}
		}

		public void DrawImage(IUGCanvasImage image, float x, float y, float width, float height)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				if (renderTarget?.Native != null)
				{
					var cgImage = renderTarget.Native;
					var destRect = new CGRect(x, y, width, height);
					Native.DrawImage(destRect, cgImage);
				}
			}
		}

		public void FillCircle(float centerX, float centerY, float radius, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.FillEllipseInRect(new CGRect(centerX - radius, centerY - radius, 2F * radius, 2F * radius));
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillCircle(float centerX, float centerY, float radius, IUGBrush brush)
		{
			if (brush is IUGSolidColorBrush solidColor)
			{
				FillCircle(centerX, centerY, radius, solidColor.Color);
				return;
			}

			var native = Native;
			var x = centerX - radius;
			var y = centerY - radius;
			var length = 2F * radius;
			try
			{
				native.SaveState();
				native.TranslateCTM(x, y);
				native.ScaleCTM(length, length);
				using (var path = CGPath.EllipseFromRect(new CGRect(0F, 0F, 1F, 1F)))
				{
					native.AddPath(path);
					native.Clip();

					var shading = ((IUGBrushInternal)brush).Native;
					native.DrawShading(shading);
				}
			}
			finally { native.RestoreState(); }
		}

		public void FillCircleInRectangle(float x, float y, float length, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.FillEllipseInRect(new CGRect(x, y, length, length));
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillCircleInRectangle(float x, float y, float length, IUGBrush brush)
		{
			if (brush is IUGSolidColorBrush solidColor)
			{
				FillCircleInRectangle(x, y, length, solidColor.Color);
				return;
			}

			var native = Native;
			try
			{
				native.SaveState();
				native.TranslateCTM(x, y);
				native.ScaleCTM(length, length);
				using (var path = CGPath.EllipseFromRect(new CGRect(0F, 0F, 1F, 1F)))
				{
					native.AddPath(path);
					native.Clip();

					var shading = ((IUGBrushInternal)brush).Native;
					native.DrawShading(shading);
				}
			}
			finally { native.RestoreState(); }
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.FillEllipseInRect(new CGRect(centerX - radiusX, centerY - radiusY, 2F * radiusX, 2F * radiusY));
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, IUGBrush brush)
		{
			if (brush is IUGSolidColorBrush solidColor)
			{
				FillEllipse(centerX, centerY, radiusX, radiusY, solidColor.Color);
				return;
			}

			var native = Native;
			var x = centerX - radiusX;
			var y = centerY - radiusY;
			var width = 2F * radiusX;
			var height = 2F * radiusY;
			try
			{
				native.SaveState();
				native.TranslateCTM(x, y);
				native.ScaleCTM(width, height);
				using (var path = CGPath.EllipseFromRect(new CGRect(0F, 0F, 1F, 1F)))
				{
					native.AddPath(path);
					native.Clip();

					var shading = ((IUGBrushInternal)brush).Native;
					native.DrawShading(shading);
				}
			}
			finally { native.RestoreState(); }
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.FillEllipseInRect(new CGRect(x, y, width, height));
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			if (brush is IUGSolidColorBrush solidColor)
			{
				FillEllipseInRectangle(x, y, width, height, solidColor.Color);
				return;
			}

			var native = Native;
			try
			{
				native.SaveState();
				native.TranslateCTM(x, y);
				native.ScaleCTM(width, height);
				using (var path = CGPath.EllipseFromRect(new CGRect(0F, 0F, 1F, 1F)))
				{
					native.AddPath(path);
					native.Clip();

					var shading = ((IUGBrushInternal)brush).Native;
					native.DrawShading(shading);
				}
			}
			finally { native.RestoreState(); }
		}

		public void FillPath(IUGPath path, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.AddPath(((UGPath)path).Native);
					native.FillPath();
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillPath(IUGPath path, float x, float y, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.TranslateCTM(x, y);
					native.AddPath(((UGPath)path).Native);
					native.FillPath();
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillRectangle(float x, float y, float width, float height, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					native.FillRect(new CGRect(x, y, width, height));
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			if (brush is IUGSolidColorBrush solidColor)
			{
				FillRectangle(x, y, width, height, solidColor.Color);
				return;
			}

			var native = Native;
			try
			{
				native.SaveState();
				native.TranslateCTM(x, y);
				native.ScaleCTM(width, height);
				native.ClipToRect(new CGRect(0, 0, 1, 1));

				var shading = ((IUGBrushInternal)brush).Native;
				native.DrawShading(shading);
			}
			finally { native.RestoreState(); }
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color)
		{
			var native = Native;
			using (var nativeColor = color.ToCGColor())
			{
				try
				{
					native.SaveState();
					native.SetFillColor(nativeColor);
					using (var path = CGPath.FromRoundedRect(new CGRect(x, y, width, height), radiusX, radiusY))
					{
						native.AddPath(path);
						native.FillPath();
					}
				}
				finally { native.RestoreState(); }
			}
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, IUGBrush brush)
		{
			if (brush is IUGSolidColorBrush solidColor)
			{
				FillRoundedRectangle(x, y, width, height, radiusX, radiusY, solidColor.Color);
				return;
			}

			var native = Native;
			try
			{
				native.SaveState();
				native.TranslateCTM(x, y);
				native.ScaleCTM(width, height);
				using (var path = CGPath.FromRoundedRect(new CGRect(0, 0, 1, 1), radiusX / width, radiusY / height))
				{
					native.AddPath(path);
					native.Clip();

					var shading = ((IUGBrushInternal)brush).Native;
					native.DrawShading(shading);
				}
			}
			finally { native.RestoreState(); }
		}

		public void Rotate(float degrees)
		{
			var radians = MathHelper.DegreesToRadians(degrees);
			Native.RotateCTM(radians);
		}

		public void Rotate(float degrees, float centerX, float centerY)
		{
			var radians = MathHelper.DegreesToRadians(degrees);
			Native.ConcatCTM(CGAffineTransformHelper.CreateRotation(radians, centerX, centerY));
		}

		public void RotateRadians(float radians) => Native.RotateCTM(radians);
		public void RotateRadians(float radians, float centerX, float centerY)
			=> Native.ConcatCTM(CGAffineTransformHelper.CreateRotation(radians, centerX, centerY));

		public void Scale(float scaleX, float scaleY) => Native.ScaleCTM(scaleX, scaleY);
		public void Scale(float scaleX, float scaleY, float centerX, float centerY)
			=> Native.ConcatCTM(CGAffineTransformHelper.CreateScale(scaleX, scaleY, centerX, centerY));

		public void Skew(float degreesX, float degreesY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
			Native.ConcatCTM(CGAffineTransformHelper.CreateSkew(radiansX, radiansY));
		}

		public void Skew(float degreesX, float degreesY, float centerX, float centerY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
			Native.ConcatCTM(CGAffineTransformHelper.CreateSkew(radiansX, radiansY, centerX, centerY));
		}

		public void Transform(Matrix3x2 transformMatrix)
		{
			Native.ConcatCTM(new CGAffineTransform(
				transformMatrix.M11, transformMatrix.M21,
				transformMatrix.M12, transformMatrix.M22,
				transformMatrix.M31, transformMatrix.M32));
		}

		public void SkewRadians(float radiansX, float radiansY)
			=> Native.ConcatCTM(CGAffineTransformHelper.CreateSkew(radiansX, radiansY));
		public void SkewRadians(float radiansX, float radiansY, float centerX, float centerY)
			=> Native.ConcatCTM(CGAffineTransformHelper.CreateSkew(radiansX, radiansY, centerX, centerY));

		public void Translate(float translateX, float translateY) => Native.TranslateCTM(translateX, translateY);

		#endregion

		public static implicit operator CGContext(UGContext d)
			=> d.Native;
	}
}
