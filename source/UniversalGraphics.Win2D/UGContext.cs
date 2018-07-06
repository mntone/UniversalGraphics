using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;

#if WINDOWS_APP || WINDOWS_PHONE_APP
using Microsoft.Graphics.Canvas.Numerics;
#else
using System.Numerics;
#endif

namespace UniversalGraphics.Win2D
{
	public sealed class UGContext : IUGContext2
	{
		object IUGObject.Native => Native;
		public CanvasDrawingSession Native { get; }

		internal CanvasDevice Device { get; }
		private readonly Size _canvasSize;
		private readonly Action _disposeAction;

		public UGContext(CanvasDevice device, CanvasDrawingSession native, Size canvasSize)
			: this(device, native, canvasSize, null)
		{ }

		public UGContext(CanvasDevice device, CanvasDrawingSession native, Size canvasSize, Action disposeAction)
		{
			Device = device;
			Native = native;
			_canvasSize = canvasSize;
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
			get => Native.Antialiasing == CanvasAntialiasing.Antialiased;
			set => Native.Antialiasing = value ? CanvasAntialiasing.Antialiased : CanvasAntialiasing.Aliased;
		}

		public UGSize CanvasSize => new UGSize((float)_canvasSize.Width, (float)_canvasSize.Height);
		public float ScaleFactor { get; }
		public int Dpi => (int)(96F * ScaleFactor + .5F);

		public void Flush() => Native.Flush();

		public void ClearColor(UGColor color)
		{
			Native.Clear(color.ToWinRTColor());
		}

		public IDisposable CreateLayer()
		{
			var layer = Native.CreateLayer(1F);
			return new CanvasLayer(Native, layer, Native.Transform);
		}
		public IDisposable CreateLayer(float alpha)
		{
			var layer = Native.CreateLayer(alpha);
			return new CanvasLayer(Native, layer, Native.Transform);
		}
		public IDisposable CreateLayer(UGRect clipRect)
		{
			var layer = Native.CreateLayer(1F, clipRect.ToWinRTRect());
			return new CanvasLayer(Native, layer, Native.Transform);
		}
		public IDisposable CreateLayer(float alpha, UGRect clipRect)
		{
			var layer = Native.CreateLayer(alpha, clipRect.ToWinRTRect());
			return new CanvasLayer(Native, layer, Native.Transform);
		}

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth)
			=> Native.DrawLine(startX, startY, endX, endY, color.ToWinRTColor(), strokeWidth);

		public void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> Native.DrawLine(startX, startY, endX, endY, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth)
			=> Native.DrawCircle(centerX, centerY, radius, color.ToWinRTColor(), strokeWidth);

		public void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> Native.DrawCircle(centerX, centerY, radius, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth)
		{
			var radius = length / 2F;
			Native.DrawEllipse(x + radius, y + radius, radius, radius, color.ToWinRTColor(), strokeWidth);
		}

		public void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var radius = length / 2F;
			Native.DrawEllipse(x + radius, y + radius, radius, radius, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());
		}

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth)
			=> Native.DrawEllipse(centerX, centerY, radiusX, radiusY, color.ToWinRTColor(), strokeWidth);

		public void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> Native.DrawEllipse(centerX, centerY, radiusX, radiusY, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
		{
			var radiusX = width / 2F;
			var radiusY = height / 2F;
			Native.DrawEllipse(x + radiusX, y + radiusY, radiusX, radiusY, color.ToWinRTColor(), strokeWidth);
		}

		public void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
		{
			var radiusX = width / 2F;
			var radiusY = height / 2F;
			Native.DrawEllipse(x + radiusX, y + radiusY, radiusX, radiusY, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());
		}

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth)
			=> Native.DrawGeometry(((UGPath)path).Native, color.ToWinRTColor(), strokeWidth);

		public void DrawPath(IUGPath path, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> Native.DrawGeometry(((UGPath)path).Native, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth)
			=> Native.DrawGeometry(((UGPath)path).Native, x, y, color.ToWinRTColor(), strokeWidth);

		public void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> Native.DrawGeometry(((UGPath)path).Native, x, y, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth)
			=> Native.DrawRectangle(x, y, width, height, color.ToWinRTColor(), strokeWidth);

		public void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> Native.DrawRectangle(x, y, width, height, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth)
			=> Native.DrawRoundedRectangle(x, y, width, height, radiusX, radiusY, color.ToWinRTColor(), strokeWidth);

		public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> Native.DrawRoundedRectangle(x, y, width, height, radiusX, radiusY, color.ToWinRTColor(), strokeWidth, strokeStyle.ToWin2DStrokeStyle());

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
				var dstRect = new Rect(x, y, width, height);
				Native.DrawImage(renderTarget.Native, dstRect);
			}
		}

		public void FillCircle(float centerX, float centerY, float radius, UGColor color)
			=> Native.FillCircle(centerX, centerY, radius, color.ToWinRTColor());

		public void FillCircle(float centerX, float centerY, float radius, IUGBrush brush)
		{
			var x = centerX - radius;
			var y = centerY - radius;
			using (CreateLayer())
			{
				Translate(x, y);
				Scale(radius, radius);
				Native.FillCircle(1F, 1F, 1F, ((IUGBrushInternal)brush).Native);
			}
		}
		public void FillCircleInRectangle(float x, float y, float length, UGColor color)
		{
			var radius = length / 2F;
			Native.FillEllipse(x + radius, y + radius, radius, radius, color.ToWinRTColor());
		}

		public void FillCircleInRectangle(float x, float y, float length, IUGBrush brush)
		{
			using (CreateLayer())
			{
				Translate(x, y);
				Scale(length, length);
				Native.FillCircle(.5F, .5F, .5F, ((IUGBrushInternal)brush).Native);
			}
		}

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color)
			=> Native.FillEllipse(centerX, centerY, radiusX, radiusY, color.ToWinRTColor());

		public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, IUGBrush brush)
		{
			var x = centerX - radiusX;
			var y = centerY - radiusY;
			using (CreateLayer())
			{
				Translate(x, y);
				Scale(radiusX, radiusY);
				Native.FillCircle(1F, 1F, 1F, ((IUGBrushInternal)brush).Native);
			}
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, UGColor color)
		{
			var radiusX = width / 2F;
			var radiusY = height / 2F;
			Native.FillEllipse(x + radiusX, y + radiusY, radiusX, radiusY, color.ToWinRTColor());
		}

		public void FillEllipseInRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			using (CreateLayer())
			{
				Translate(x, y);
				Scale(width, height);
				Native.FillCircle(.5F, .5F, .5F, ((IUGBrushInternal)brush).Native);
			}
		}

		public void FillPath(IUGPath path, UGColor color)
			=> Native.FillGeometry(((UGPath)path).Native, color.ToWinRTColor());

		public void FillPath(IUGPath path, float x, float y, UGColor color)
			=> Native.FillGeometry(((UGPath)path).Native, x, y, color.ToWinRTColor());

		public void FillRectangle(float x, float y, float width, float height, UGColor color)
			=> Native.FillRectangle(x, y, width, height, color.ToWinRTColor());

		public void FillRectangle(float x, float y, float width, float height, IUGBrush brush)
		{
			using (CreateLayer())
			{
				Translate(x, y);
				Scale(width, height);
				Native.FillRectangle(0F, 0F, 1F, 1F, ((IUGBrushInternal)brush).Native);
			}
		}

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color)
			=> Native.FillRoundedRectangle(x, y, width, height, radiusX, radiusY, color.ToWinRTColor());

		public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, IUGBrush brush)
		{
			using (CreateLayer())
			{
				Translate(x, y);
				Scale(width, height);
				Native.FillRoundedRectangle(0F, 0F, 1F, 1F, radiusX / width, radiusY / height, ((IUGBrushInternal)brush).Native);
			}
		}

		public void Rotate(float degrees)
		{
			var radians = MathHelper.DegreesToRadians(degrees);
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateRotation(radians);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateRotation(radians) * Native.Transform;
#endif
		}

		public void Rotate(float degrees, float centerX, float centerY)
		{
			var radians = MathHelper.DegreesToRadians(degrees);
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateRotation(radians, centerX, centerY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateRotation(radians, new Vector2(centerX, centerY)) * Native.Transform;
#endif
		}

		public void RotateRadians(float radians)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateRotation(radians);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateRotation(radians) * Native.Transform;
#endif
		}

		public void RotateRadians(float radians, float centerX, float centerY)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateRotation(radians, centerX, centerY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateRotation(radians, new Vector2(centerX, centerY)) * Native.Transform;
#endif
		}

		public void Scale(float scaleX, float scaleY)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateScale(scaleX, scaleY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateScale(scaleX, scaleY) * Native.Transform;
#endif
		}

		public void Scale(float scaleX, float scaleY, float centerX, float centerY)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateScale(scaleX, scaleY, centerX, centerY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateScale(scaleX, scaleY, new Vector2(centerX, centerY)) * Native.Transform;
#endif
		}

		public void SkewRadians(float radiansX, float radiansY)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateSkew(radiansX, radiansY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateSkew(radiansX, radiansY) * Native.Transform;
#endif
		}

		public void SkewRadians(float radiansX, float radiansY, float centerX, float centerY)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateSkew(radiansX, radiansY, centerX, centerY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateSkew(radiansX, radiansY, new Vector2(centerX, centerY)) * Native.Transform;
#endif
		}

		public void Skew(float degreesX, float degreesY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateSkew(radiansX, radiansY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateSkew(radiansX, radiansY) * Native.Transform;
#endif
		}

		public void Skew(float degreesX, float degreesY, float centerX, float centerY)
		{
			var radiansX = MathHelper.DegreesToRadians(degreesX);
			var radiansY = MathHelper.DegreesToRadians(degreesY);
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateSkew(radiansX, radiansY, centerX, centerY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateSkew(radiansX, radiansY, new Vector2(centerX, centerY)) * Native.Transform;
#endif
		}

		public void Transform(Matrix3x2 transformMatrix)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			Native.Transform = Matrix3x2Helper.Multiply(transformMatrix, Native.Transform);
#else
			Native.Transform = transformMatrix * Native.Transform;
#endif
		}

		public void Translate(float translateX, float translateY)
		{
#if WINDOWS_APP || WINDOWS_PHONE_APP
			var matrix = Matrix3x2Helper.CreateTranslate(translateX, translateY);
			Native.Transform = Matrix3x2Helper.Multiply(matrix, Native.Transform);
#else
			Native.Transform = Matrix3x2.CreateTranslation(translateX, translateY) * Native.Transform;
#endif
		}

		#endregion

		public static implicit operator CanvasDrawingSession(UGContext d)
			=> d.Native;
	}
}
