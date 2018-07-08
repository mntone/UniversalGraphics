using Android.Graphics;
using Android.OS;
using System;
using System.Numerics;

namespace UniversalGraphics.Droid2D
{
	public sealed class UGContext : IUGContext2
	{
		object IUGObject.Native => Native;
		public Canvas Native { get; }

		private readonly Paint _context;
		private readonly Action _disposeAction;

		internal UGContext(Canvas native, UGSize canvasRect, float scale)
			: this(native, canvasRect, scale, null)
		{ }

		internal UGContext(Canvas native, UGSize canvasRect, float scale, Action disposeAction)
		{
			Native = native;
			CanvasSize = canvasRect;
			ScaleFactor = scale;
			_disposeAction = disposeAction;

			_context = new Paint();
			Antialiasing = true;
		}

		public void Dispose()
		{
			_context.Dispose();
			_disposeAction?.Invoke();
			GC.SuppressFinalize(this);
		}

		#region IUGContext Methods

		IUGFactory IUGContext.Factory => new UGFactory(this);

		public bool Antialiasing
		{
			get => _context.AntiAlias;
			set => _context.AntiAlias = value;
		}

		public UGTextAntialiasing TextAntialiasing { get; set; }

		public UGSize CanvasSize { get; }
		public float ScaleFactor { get; }
		public int Dpi => (int)(160F * ScaleFactor + .5F);

		public void Flush() { }

		public void ClearColor(UGColor color)
		{
			var nativeColor = color.ToAGColor();
			Native.DrawColor(nativeColor);
		}

		public IDisposable CreateLayer()
		{
			var native = Native;
			var count = native.Save();
			return Disposable.Create(() => native.RestoreToCount(count));
		}

#pragma warning disable CS0618
		public IDisposable CreateLayer(float alpha)
		{
			var alphaInt = (int)(255F * alpha + .5F);
			var native = Native;
			int count = -1;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				count = native.SaveLayerAlpha(null, alphaInt);
			}
			else
			{
				count = native.SaveLayerAlpha(null, alphaInt, SaveFlags.All);
			}
			return Disposable.Create(() =>
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
				else
				{
					native.Restore();
				}
			});
		}

		public IDisposable CreateLayer(UGRect clipRect)
		{
			var native = Native;
			int count = -1;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				count = native.SaveLayer(clipRect.Left, clipRect.Top, clipRect.Right, clipRect.Bottom, null);
			}
			else
			{
				count = native.SaveLayer(clipRect.Left, clipRect.Top, clipRect.Right, clipRect.Bottom, null, SaveFlags.All);
			}
			return Disposable.Create(() =>
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
				else
				{
					native.Restore();
				}
			});
		}

		public IDisposable CreateLayer(float alpha, UGRect clipRect)
		{
			var alphaInt = (int)(255F * alpha + .5F);
			var native = Native;
			int count = -1;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				count = native.SaveLayerAlpha(clipRect.Left, clipRect.Top, clipRect.Right, clipRect.Bottom, alphaInt);
			}
			else
			{
				count = native.SaveLayerAlpha(clipRect.Left, clipRect.Top, clipRect.Right, clipRect.Bottom, alphaInt, SaveFlags.All);
			}
			return Disposable.Create(() =>
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
				else
				{
					native.Restore();
				}
			});
		}
#pragma warning restore CS0618

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawLine(startX, startY, endX, endY, paint);
			}
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				if (strokeStyle.DashStyle.Value != null)
				{
					using (var line = new Path())
					{
						line.MoveTo(startX, startY);
						line.LineTo(endX, endY);
						Native.DrawPath(line, paint);
					}
				}
				else
				{
					Native.DrawLine(startX, startY, endX, endY, paint);
				}
			}
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawCircle(centerX, centerY, radius, paint);
			}
		}

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				Native.DrawCircle(centerX, centerY, radius, paint);
			}
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawOval(x, y, x + length, y + length, paint);
			}
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				Native.DrawOval(x, y, x + length, y + length, paint);
			}
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawOval(centerX - radiusX, centerY - radiusY, centerX + radiusX, centerY + radiusY, paint);
			}
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				Native.DrawOval(centerX - radiusX, centerY - radiusY, centerX + radiusX, centerY + radiusY, paint);
			}
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawOval(x, y, x + width, y + height, paint);
			}
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				Native.DrawOval(x, y, x + width, y + height, paint);
			}
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawPath(((UGPath)path).Native, paint);
			}
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				Native.DrawPath(((UGPath)path).Native, paint);
			}
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;

				var native = Native;
				int count = -1;
				try
				{
					count = native.Save();
					native.Translate(x, y);
					native.DrawPath(((UGPath)path).Native, paint);
				}
				finally
				{
					if (count != -1)
					{
						native.RestoreToCount(count);
					}
				}
			}
		}

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);

				var native = Native;
				int count = -1;
				try
				{
					count = native.Save();
					native.Translate(x, y);
					native.DrawPath(((UGPath)path).Native, paint);
				}
				finally
				{
					if (count != -1)
					{
						native.RestoreToCount(count);
					}
				}
			}
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawRect(x, y, x + width, y + height, paint);
			}
		}

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				Native.DrawRect(x, y, x + width, y + height, paint);
			}
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				Native.DrawRoundRect(x, y, x + width, y + height, radiusX, radiusY, paint);
			}
		}

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Stroke);
				paint.StrokeWidth = strokeWidth;
				paint.SetStrokeStyle(strokeWidth, strokeStyle);
				Native.DrawRoundRect(x, y, x + width, y + height, radiusX, radiusY, paint);
			}
		}

		public void DrawTextLayout(IUGTextLayout textLayout, float x, float y, UGColor color)
		{
			var pTextLayout = (UGTextLayout)textLayout;
			pTextLayout.SetTextAntialiasing(TextAntialiasing);

			var native = Native;
			int count = -1;
			try
			{
				count = native.Save();
				native.Translate(x, y);
				pTextLayout.Draw(native, color);
			}
			finally
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
			}
		}

		public void DrawImage(IUGCanvasImage image, float x, float y)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				Native.DrawBitmap(renderTarget.Native, x, y, null);
			}
		}

		public void DrawImage(IUGCanvasImage image, float x, float y, float width, float height)
		{
			if (image is UGCanvasRenderTarget renderTarget)
			{
				var source = renderTarget.Native;
				var srcRect = new Rect(0, 0, source.Width, source.Height);
				Native.DrawBitmap(source, srcRect, new RectF(x, y, x + width, y + height), null);
			}
		}

		public void FillCircle(float centerX, float centerY, float radius, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);
				Native.DrawCircle(centerX, centerY, radius, paint);
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
			int count = -1;
			try
			{
				count = native.Save();
				using (var paint = new Paint())
				{
					paint.Set(_context);
					paint.SetShader(((IUGBrushInternal)brush).Native);
					paint.SetStyle(Paint.Style.Fill);
					native.Translate(centerX - radius, centerY - radius);
					native.Scale(centerX + radius, centerY + radius);
					native.DrawOval(0F, 0F, 1F, 1F, paint);
				}
			}
			finally
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
			}
		}

		public void FillCircleInRectangle(float x, float y, float length, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);
				Native.DrawOval(x, y, x + length, y + length, paint);
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
			int count = -1;
			try
			{
				count = native.Save();
				using (var paint = new Paint())
				{
					paint.Set(_context);
					paint.SetShader(((IUGBrushInternal)brush).Native);
					paint.SetStyle(Paint.Style.Fill);
					native.Translate(x, y);
					native.Scale(length, length);
					native.DrawOval(0F, 0F, 1F, 1F, paint);
				}
			}
			finally
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
			}
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);
				Native.DrawOval(centerX - radiusX, centerY - radiusY, centerX + radiusX, centerY + radiusY, paint);
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
			int count = -1;
			try
			{
				count = native.Save();
				using (var paint = new Paint())
				{
					paint.Set(_context);
					paint.SetShader(((IUGBrushInternal)brush).Native);
					paint.SetStyle(Paint.Style.Fill);
					native.Translate(centerX - radiusX, centerY - radiusY);
					native.Scale(centerX + radiusX, centerY + radiusY);
					native.DrawOval(0F, 0F, 1F, 1F, paint);
				}
			}
			finally
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
			}
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);
				Native.DrawOval(x, y, x + width, y + height, paint);
			}
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			if (brush is IUGSolidColorBrush solidColor)
			{
				FillEllipse(x, y, width, height, solidColor.Color);
				return;
			}

			var native = Native;
			int count = -1;
			try
			{
				count = native.Save();
				using (var paint = new Paint())
				{
					paint.Set(_context);
					paint.SetShader(((IUGBrushInternal)brush).Native);
					paint.SetStyle(Paint.Style.Fill);
					native.Translate(x, y);
					native.Scale(width, height);
					native.DrawOval(0F, 0F, 1F, 1F, paint);
				}
			}
			finally
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
			}
		}

		public void FillPath(IUGPath path, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);
				Native.DrawPath(((UGPath)path).Native, paint);
			}
		}

		public void FillPath(IUGPath path, float x, float y, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);

				var native = Native;
				int count = -1;
				try
				{
					count = native.Save();
					native.Translate(x, y);
					native.DrawPath(((UGPath)path).Native, paint);
				}
				finally
				{
					if (count != -1)
					{
						native.RestoreToCount(count);
					}
				}
			}
		}

		public void FillRectangle(float x, float y, float width, float height, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);
				Native.DrawRect(x, y, x + width, y + height, paint);
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
			int count = -1;
			try
			{
				count = native.Save();
				using (var paint = new Paint())
				{
					paint.Set(_context);
					paint.SetShader(((IUGBrushInternal)brush).Native);
					paint.SetStyle(Paint.Style.Fill);
					native.Translate(x, y);
					native.Scale(width, height);
					native.DrawRect(0F, 0F, 1F, 1F, paint);
				}
			}
			finally
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
			}
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color)
		{
			using (var paint = new Paint())
			{
				paint.Set(_context);
				paint.Color = color.ToAGColor();
				paint.SetStyle(Paint.Style.Fill);
				Native.DrawRoundRect(x, y, x + width, y + height, radiusX, radiusY, paint);
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
			int count = -1;
			try
			{
				count = native.Save();
				using (var paint = new Paint())
				{
					paint.Set(_context);
					paint.SetShader(((IUGBrushInternal)brush).Native);
					paint.SetStyle(Paint.Style.Fill);
					native.Translate(x, y);
					native.Scale(width, height);
					Native.DrawRoundRect(0F, 0F, 1F, 1F, radiusX / width, radiusY / height, paint);
				}
			}
			finally
			{
				if (count != -1)
				{
					native.RestoreToCount(count);
				}
			}
		}

		public void Rotate(float degrees) => Native.Rotate(degrees);
		public void Rotate(float degrees, float centerX, float centerY) => Native.Rotate(degrees, centerX, centerY);

		public void RotateRadians(float radians)
		{
			var degrees = MathHelper.RadiansToDegrees(radians);
			Native.Rotate(degrees);
		}

		public void RotateRadians(float radians, float centerX, float centerY)
		{
			var degrees = MathHelper.RadiansToDegrees(radians);
			Native.Rotate(degrees, centerX, centerY);
		}

		public void Scale(float scaleX, float scaleY) => Native.Scale(scaleX, scaleY);
		public void Scale(float scaleX, float scaleY, float centerX, float centerY)
			=> Native.Scale(scaleX, scaleY, centerX, centerY);

		public void Skew(float degreesX, float degreesY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			Native.Skew(skewX, skewY);
		}

		public void Skew(float degreesX, float degreesY, float centerX, float centerY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			using (var matrix = new Matrix())
			{
				matrix.SetSkew(skewX, skewY, centerX, centerY);
				Native.Concat(matrix);
			}
		}

		public void SkewRadians(float radiansX, float radiansY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			Native.Skew(skewX, skewY);
		}

		public void SkewRadians(float radiansX, float radiansY, float centerX, float centerY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			using (var matrix = new Matrix())
			{
				matrix.SetSkew(skewX, skewY, centerX, centerY);
				Native.Concat(matrix);
			}
		}

		public void Transform(Matrix3x2 transformMatrix)
		{
			using (var matrix = new Matrix())
			{
				matrix.SetValues(new[] {
					transformMatrix.M11, transformMatrix.M21, transformMatrix.M31,
					transformMatrix.M12, transformMatrix.M22, transformMatrix.M32,
					0F, 0F, 1F });
				Native.Concat(matrix);
			}
		}

		public void Translate(float translateX, float translateY) => Native.Translate(translateX, translateY);

		#endregion

		public static implicit operator Canvas(UGContext d)
			=> d.Native;
	}
}
