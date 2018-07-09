using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UniversalGraphics
{
	public interface IUGContext : IUGObject
	{
		IUGFactory Factory { get; }

		bool Antialiasing { get; set; }
		UGTextAntialiasing TextAntialiasing { get; set; }

		UGSize CanvasSize { get; }
		float ScaleFactor { get; }
		int Dpi { get; }

		void Flush();

		void ClearColor(UGColor color);

		IDisposable CreateLayer();
		IDisposable CreateLayer(UGRect clipRect);

		void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth);
		void DrawLine(float startX, float startY, float endX, float endY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawLines(IEnumerable<Vector2> points, UGColor color, float strokeWidth);
		void DrawLines(IEnumerable<Vector2> points, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth);
		void DrawCircle(float centerX, float centerY, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth);
		void DrawCircleInRectangle(float x, float y, float length, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth);
		void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth);
		void DrawEllipseInRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawPath(IUGPath path, UGColor color, float strokeWidth);
		void DrawPath(IUGPath path, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);
		void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth);
		void DrawPath(IUGPath path, float x, float y, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth);
		void DrawRectangle(float x, float y, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth);
		void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle);

		void DrawTextLayout(IUGTextLayout textLayout, float x, float y, UGColor color);

		void DrawImage(IUGCanvasImage image, float x, float y);
		void DrawImage(IUGCanvasImage image, float x, float y, float width, float height);

		void FillCircle(float centerX, float centerY, float radius, UGColor color);
		void FillCircle(float centerX, float centerY, float radius, IUGBrush brush);
		void FillCircleInRectangle(float x, float y, float length, UGColor color);
		void FillCircleInRectangle(float x, float y, float length, IUGBrush brush);
		void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, UGColor color);
		void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, IUGBrush brush);
		void FillEllipseInRectangle(float x, float y, float width, float height, UGColor color);
		void FillEllipseInRectangle(float x, float y, float width, float height, IUGBrush brush);
		void FillPath(IUGPath path, UGColor color);
		void FillPath(IUGPath path, float x, float y, UGColor color);
		void FillRectangle(float x, float y, float width, float height, UGColor color);
		void FillRectangle(float x, float y, float width, float height, IUGBrush brush);
		void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, UGColor color);
		void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, IUGBrush brush);

		void Rotate(float degrees);
		void Rotate(float degrees, float centerX, float centerY);
		void RotateRadians(float radians);
		void RotateRadians(float radians, float centerX, float centerY);
		void Scale(float scaleX, float scaleY);
		void Scale(float scaleX, float scaleY, float centerX, float centerY);
		void Skew(float degreesX, float degreesY);
		void Skew(float degreesX, float degreesY, float centerX, float centerY);
		void SkewRadians(float radiansX, float radiansY);
		void SkewRadians(float radiansX, float radiansY, float centerX, float centerY);
		void Transform(Matrix3x2 transformMatrix);
		void Translate(float translateX, float translateY);
	}

	public interface IUGContext2 : IUGContext
	{
		IDisposable CreateLayer(float alpha);
		IDisposable CreateLayer(float alpha, UGRect clipRect);
	}

	public static class IUGContextExtensions
	{
		#region DrawLine

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, float startX, float startY, float endX, float endY, UGColor color)
			=> context.DrawLine(startX, startY, endX, endY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, float startX, float startY, Vector2 end, UGColor color)
			=> context.DrawLine(startX, startY, end.X, end.Y, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, float startX, float startY, Vector2 end, UGColor color, float strokeWidth)
			=> context.DrawLine(startX, startY, end.X, end.Y, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, float startX, float startY, Vector2 end, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawLine(startX, startY, end.X, end.Y, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, Vector2 start, float endX, float endY, UGColor color)
			=> context.DrawLine(start.X, start.Y, endX, endY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, Vector2 start, float endX, float endY, UGColor color, float strokeWidth)
			=> context.DrawLine(start.X, start.Y, endX, endY, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, Vector2 start, float endX, float endY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawLine(start.X, start.Y, endX, endY, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, Vector2 start, Vector2 end, UGColor color)
			=> context.DrawLine(start.X, start.Y, end.X, end.Y, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, Vector2 start, Vector2 end, UGColor color, float strokeWidth)
			=> context.DrawLine(start.X, start.Y, end.X, end.Y, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(this IUGContext context, Vector2 start, Vector2 end, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawLine(start.X, start.Y, end.X, end.Y, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawLines

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLines(this IUGContext context, IEnumerable<Vector2> points, UGColor color)
			=> context.DrawLines(points, color, 1F);

		#endregion

		#region DrawCircle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircle(this IUGContext context, float centerX, float centerY, float radius, UGColor color)
			=> context.DrawCircle(centerX, centerY, radius, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircle(this IUGContext context, Vector2 centerPoint, float radius, UGColor color)
			=> context.DrawCircle(centerPoint.X, centerPoint.Y, radius, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircle(this IUGContext context, Vector2 centerPoint, float radius, UGColor color, float strokeWidth)
			=> context.DrawCircle(centerPoint.X, centerPoint.Y, radius, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircle(this IUGContext context, Vector2 centerPoint, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawCircle(centerPoint.X, centerPoint.Y, radius, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawEllipseInRectangle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircleInRectangle(this IUGContext context, float x, float y, float length, UGColor color)
			=> context.DrawCircleInRectangle(x, y, length, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircleInRectangle(this IUGContext context, Vector2 point, float length, UGColor color)
			=> context.DrawCircleInRectangle(point.X, point.Y, length, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircleInRectangle(this IUGContext context, Vector2 point, float length, UGColor color, float strokeWidth)
			=> context.DrawCircleInRectangle(point.X, point.Y, length, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawCircleInRectangle(this IUGContext context, Vector2 point, float length, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawCircleInRectangle(point.X, point.Y, length, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawEllipse

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipse(this IUGContext context, float centerX, float centerY, float radiusX, float radiusY, UGColor color)
			=> context.DrawEllipse(centerX, centerY, radiusX, radiusY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipse(this IUGContext context, Vector2 centerPoint, float radiusX, float radiusY, UGColor color)
			=> context.DrawEllipse(centerPoint.X, centerPoint.Y, radiusX, radiusY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipse(this IUGContext context, Vector2 centerPoint, float radiusX, float radiusY, UGColor color, float strokeWidth)
			=> context.DrawEllipse(centerPoint.X, centerPoint.Y, radiusX, radiusY, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipse(this IUGContext context, Vector2 centerPoint, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawEllipse(centerPoint.X, centerPoint.Y, radiusX, radiusY, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawEllipseInRectangle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, float x, float y, float width, float height, UGColor color)
			=> context.DrawEllipseInRectangle(x, y, width, height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color)
			=> context.DrawEllipseInRectangle(x, y, size.Width, size.Height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color, float strokeWidth)
			=> context.DrawEllipseInRectangle(x, y, size.Width, size.Height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawEllipseInRectangle(x, y, size.Width, size.Height, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color)
			=> context.DrawEllipseInRectangle(point.X, point.Y, width, height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color, float strokeWidth)
			=> context.DrawEllipseInRectangle(point.X, point.Y, width, height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawEllipseInRectangle(point.X, point.Y, width, height, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color)
			=> context.DrawEllipseInRectangle(point.X, point.Y, size.Width, size.Height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color, float strokeWidth)
			=> context.DrawEllipseInRectangle(point.X, point.Y, size.Width, size.Height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawEllipseInRectangle(point.X, point.Y, size.Width, size.Height, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, UGRect rect, UGColor color)
			=> context.DrawEllipseInRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, UGRect rect, UGColor color, float strokeWidth)
			=> context.DrawEllipseInRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawEllipseInRectangle(this IUGContext context, UGRect rect, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawEllipseInRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawPath

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPath(this IUGContext context, IUGPath path, UGColor color)
			=> context.DrawPath(path, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPath(this IUGContext context, IUGPath path, UGColor color, float strokeWidth)
			=> context.DrawPath(path, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPath(this IUGContext context, IUGPath path, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawPath(path, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPath(this IUGContext context, IUGPath path, float x, float y, UGColor color)
			=> context.DrawPath(path, x, y, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPath(this IUGContext context, IUGPath path, Vector2 offset, UGColor color)
			=> context.DrawPath(path, offset.X, offset.Y, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPath(this IUGContext context, IUGPath path, Vector2 offset, UGColor color, float strokeWidth)
			=> context.DrawPath(path, offset.X, offset.Y, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPath(this IUGContext context, IUGPath path, Vector2 offset, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawPath(path, offset.X, offset.Y, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawRectangle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, float x, float y, float width, float height, UGColor color)
			=> context.DrawRectangle(x, y, width, height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color)
			=> context.DrawRectangle(x, y, size.Width, size.Height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color, float strokeWidth)
			=> context.DrawRectangle(x, y, size.Width, size.Height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRectangle(x, y, size.Width, size.Height, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color)
			=> context.DrawRectangle(point.X, point.Y, width, height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color, float strokeWidth)
			=> context.DrawRectangle(point.X, point.Y, width, height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRectangle(point.X, point.Y, width, height, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color)
			=> context.DrawRectangle(point.X, point.Y, size.Width, size.Height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color, float strokeWidth)
			=> context.DrawRectangle(point.X, point.Y, size.Width, size.Height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRectangle(point.X, point.Y, size.Width, size.Height, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, UGRect rect, UGColor color)
			=> context.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, UGRect rect, UGColor color, float strokeWidth)
			=> context.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRectangle(this IUGContext context, UGRect rect, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawRoundedRectangle (radius only)

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, float width, float height, float radius, UGColor color)
			=> context.DrawRoundedRectangle(x, y, width, height, radius, radius, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radius, UGColor color)
			=> context.DrawRoundedRectangle(x, y, size.Width, size.Height, radius, radius, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radius, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(x, y, size.Width, size.Height, radius, radius, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(x, y, size.Width, size.Height, radius, radius, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radius, UGColor color)
			=> context.DrawRoundedRectangle(point.X, point.Y, width, height, radius, radius, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radius, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(point.X, point.Y, width, height, radius, radius, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(point.X, point.Y, width, height, radius, radius, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radius, UGColor color)
			=> context.DrawRoundedRectangle(point.X, point.Y, size.Width, size.Height, radius, radius, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radius, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(point.X, point.Y, size.Width, size.Height, radius, radius, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(point.X, point.Y, size.Width, size.Height, radius, radius, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, UGRect rect, float radius, UGColor color)
			=> context.DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius, radius, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, UGRect rect, float radius, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius, radius, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, UGRect rect, float radius, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius, radius, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawRoundedRectangle (radiusX & radiusY)

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, float width, float height, float radiusX, float radiusY, UGColor color)
			=> context.DrawRoundedRectangle(x, y, width, height, radiusX, radiusY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radiusX, float radiusY, UGColor color)
			=> context.DrawRoundedRectangle(x, y, size.Width, size.Height, radiusX, radiusY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radiusX, float radiusY, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(x, y, size.Width, size.Height, radiusX, radiusY, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(x, y, size.Width, size.Height, radiusX, radiusY, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radiusX, float radiusY, UGColor color)
			=> context.DrawRoundedRectangle(point.X, point.Y, width, height, radiusX, radiusY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(point.X, point.Y, width, height, radiusX, radiusY, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(point.X, point.Y, width, height, radiusX, radiusY, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radiusX, float radiusY, UGColor color)
			=> context.DrawRoundedRectangle(point.X, point.Y, size.Width, size.Height, radiusX, radiusY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radiusX, float radiusY, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(point.X, point.Y, size.Width, size.Height, radiusX, radiusY, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(point.X, point.Y, size.Width, size.Height, radiusX, radiusY, color, strokeWidth, strokeStyle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, UGRect rect, float radiusX, float radiusY, UGColor color)
			=> context.DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radiusX, radiusY, color, 1F);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, UGRect rect, float radiusX, float radiusY, UGColor color, float strokeWidth)
			=> context.DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radiusX, radiusY, color, strokeWidth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawRoundedRectangle(this IUGContext context, UGRect rect, float radiusX, float radiusY, UGColor color, float strokeWidth, UGStrokeStyle strokeStyle)
			=> context.DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radiusX, radiusY, color, strokeWidth, strokeStyle);

		#endregion

		#region DrawTextLayout

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawTextLayout(this IUGContext context, IUGTextLayout textLayout, Vector2 point, UGColor color)
			=> context.DrawTextLayout(textLayout, point.X, point.Y, color);

		#endregion

		#region DrawImage

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawImage(this IUGContext context, IUGCanvasImage image, Vector2 offset)
			=> context.DrawImage(image, offset.X, offset.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawImage(this IUGContext context, IUGCanvasImage image, UGRect destRect)
			=> context.DrawImage(image, destRect.X, destRect.Y, destRect.Width, destRect.Height);

		#endregion


		#region FillCircle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillCircle(this IUGContext context, Vector2 centerPoint, float radius, UGColor color)
			=> context.FillCircle(centerPoint.X, centerPoint.Y, radius, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillCircle(this IUGContext context, Vector2 centerPoint, float radius, IUGBrush brush)
			=> context.FillCircle(centerPoint.X, centerPoint.Y, radius, brush);

		#endregion

		#region FillCircleInRectangle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillCircleInRectangle(this IUGContext context, Vector2 point, float length, UGColor color)
			=> context.FillCircleInRectangle(point.X, point.Y, length, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillCircleInRectangle(this IUGContext context, Vector2 point, float length, IUGBrush brush)
			=> context.FillCircleInRectangle(point.X, point.Y, length, brush);

		#endregion

		#region FillEllipse

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipse(this IUGContext context, Vector2 centerPoint, float radiusX, float radiusY, UGColor color)
			=> context.FillEllipse(centerPoint.X, centerPoint.Y, radiusX, radiusY, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipse(this IUGContext context, Vector2 centerPoint, float radiusX, float radiusY, IUGBrush brush)
			=> context.FillEllipse(centerPoint.X, centerPoint.Y, radiusX, radiusY, brush);

		#endregion

		#region FillEllipseInRectangle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color)
			=> context.FillEllipseInRectangle(x, y, size.Width, size.Height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, float x, float y, UGSize size, IUGBrush brush)
			=> context.FillEllipseInRectangle(x, y, size.Width, size.Height, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color)
			=> context.FillEllipseInRectangle(point.X, point.Y, width, height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, Vector2 point, float width, float height, IUGBrush brush)
			=> context.FillEllipseInRectangle(point.X, point.Y, width, height, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color)
		=> context.FillEllipseInRectangle(point.X, point.Y, size.Width, size.Height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, Vector2 point, UGSize size, IUGBrush brush)
			=> context.FillEllipseInRectangle(point.X, point.Y, size.Width, size.Height, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, UGRect rect, UGColor color)
			=> context.FillEllipseInRectangle(rect.X, rect.Y, rect.Width, rect.Height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillEllipseInRectangle(this IUGContext context, UGRect rect, IUGBrush brush)
			=> context.FillEllipseInRectangle(rect.X, rect.Y, rect.Width, rect.Height, brush);

		#endregion

		#region FillRectangle

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, float x, float y, UGSize size, UGColor color)
			=> context.FillRectangle(x, y, size.Width, size.Height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, float x, float y, UGSize size, IUGBrush brush)
			=> context.FillRectangle(x, y, size.Width, size.Height, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, Vector2 point, float width, float height, UGColor color)
			=> context.FillRectangle(point.X, point.Y, width, height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, Vector2 point, float width, float height, IUGBrush brush)
			=> context.FillRectangle(point.X, point.Y, width, height, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, Vector2 point, UGSize size, UGColor color)
			=> context.FillRectangle(point.X, point.Y, size.Width, size.Height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, Vector2 point, UGSize size, IUGBrush brush)
			=> context.FillRectangle(point.X, point.Y, size.Width, size.Height, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, UGRect rect, UGColor color)
			=> context.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRectangle(this IUGContext context, UGRect rect, IUGBrush brush)
		=> context.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height, brush);

		#endregion

		#region FillRoundedRectangle
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radius, UGColor color)
			=> context.FillRoundedRectangle(x, y, size.Width, size.Height, radius, radius, color);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radius, IUGBrush brush)
			=> context.FillRoundedRectangle(x, y, size.Width, size.Height, radius, radius, brush);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radius, UGColor color)
			=> context.FillRoundedRectangle(point.X, point.Y, width, height, radius, radius, color);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radius, IUGBrush brush)
			=> context.FillRoundedRectangle(point.X, point.Y, width, height, radius, radius, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radius, UGColor color)
			=> context.FillRoundedRectangle(point.X, point.Y, size.Width, size.Height, radius, radius, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radius, IUGBrush brush)
			=> context.FillRoundedRectangle(point.X, point.Y, size.Width, size.Height, radius, radius, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, UGRect rect, float radius, UGColor color)
			=> context.FillRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius, radius, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, UGRect rect, float radius, IUGBrush brush)
			=> context.FillRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius, radius, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radiusX, float radiusY, UGColor color)
			=> context.FillRoundedRectangle(x, y, size.Width, size.Height, radiusX, radiusY, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, float x, float y, UGSize size, float radiusX, float radiusY, IUGBrush brush)
			=> context.FillRoundedRectangle(x, y, size.Width, size.Height, radiusX, radiusY, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radiusX, float radiusY, UGColor color)
			=> context.FillRoundedRectangle(point.X, point.Y, width, height, radiusX, radiusY, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, float width, float height, float radiusX, float radiusY, IUGBrush brush)
			=> context.FillRoundedRectangle(point.X, point.Y, width, height, radiusX, radiusY, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radiusX, float radiusY, UGColor color)
			=> context.FillRoundedRectangle(point.X, point.Y, size.Width, size.Height, radiusX, radiusY, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, Vector2 point, UGSize size, float radiusX, float radiusY, IUGBrush brush)
			=> context.FillRoundedRectangle(point.X, point.Y, size.Width, size.Height, radiusX, radiusY, brush);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, UGRect rect, float radiusX, float radiusY, UGColor color)
			=> context.FillRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radiusX, radiusY, color);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FillRoundedRectangle(this IUGContext context, UGRect rect, float radiusX, float radiusY, IUGBrush brush)
			=> context.FillRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radiusX, radiusY, brush);

		#endregion


		#region Rotate

		public static void Rotate(this IUGContext context, float degrees, Vector2 center)
			=> context.Rotate(degrees, center.X, center.Y);

		public static void RotateRadians(this IUGContext context, float radians, Vector2 center)
			=> context.RotateRadians(radians, center.X, center.Y);

		#endregion

		#region Scale

		public static void ScaleX(this IUGContext context, float scaleX)
			=> context.Scale(scaleX, 1F);

		public static void ScaleX(this IUGContext context, float scaleX, Vector2 center)
			=> context.Scale(scaleX, 1F, center.X, center.Y);

		public static void ScaleY(this IUGContext context, float scaleY)
			=> context.Scale(1F, scaleY);

		public static void ScaleY(this IUGContext context, float scaleY, Vector2 center)
			=> context.Scale(1F, scaleY, center.X, center.Y);

		public static void Scale(this IUGContext context, float scaleX, float scaleY, Vector2 center)
			=> context.Scale(scaleX, scaleY, center.X, center.Y);

		public static void Scale(this IUGContext context, float scale)
			=> context.Scale(scale, scale);

		public static void Scale(this IUGContext context, float scale, float centerX, float centerY)
			=> context.Scale(scale, scale, centerX, centerY);

		public static void Scale(this IUGContext context, float scale, Vector2 center)
			=> context.Scale(scale, scale, center.X, center.Y);

		public static void Scale(this IUGContext context, Vector2 scale)
			=> context.Scale(scale.X, scale.Y);

		public static void Scale(this IUGContext context, Vector2 scale, float centerX, float centerY)
			=> context.Scale(scale.X, scale.Y, centerX, centerY);

		public static void Scale(this IUGContext context, Vector2 scale, Vector2 center)
			=> context.Scale(scale.X, scale.Y, center.X, center.Y);

		#endregion

		#region Skew

		public static void SkewRadiansX(this IUGContext context, float skewX)
			=> context.SkewRadians(skewX, 0F);

		public static void SkewRadiansX(this IUGContext context, float skewX, float centerX, float centerY)
			=> context.SkewRadians(skewX, 0F, centerX, centerY);

		public static void SkewRadiansX(this IUGContext context, float skewX, Vector2 center)
			=> context.SkewRadians(skewX, 0F, center.X, center.Y);

		public static void SkewRadiansY(this IUGContext context, float skewY)
			=> context.SkewRadians(0F, skewY);

		public static void SkewRadiansY(this IUGContext context, float skewY, float centerX, float centerY)
			=> context.SkewRadians(0F, skewY, centerX, centerY);

		public static void SkewRadiansY(this IUGContext context, float skewY, Vector2 center)
			=> context.SkewRadians(0F, skewY, center.X, center.Y);

		public static void SkewRadians(this IUGContext context, Vector2 skew)
			=> context.SkewRadians(skew.X, skew.Y);

		public static void SkewRadians(this IUGContext context, Vector2 skew, Vector2 center)
			=> context.SkewRadians(skew.X, skew.Y, center.X, center.Y);

		#endregion

		#region SkewDegrees

		public static void SkewX(this IUGContext context, float degreesX)
			=> context.Skew(degreesX, 0F);

		public static void SkewX(this IUGContext context, float degreesX, float centerX, float centerY)
			=> context.Skew(degreesX, 0F, centerX, centerY);

		public static void SkewX(this IUGContext context, float degreesX, Vector2 center)
			=> context.Skew(degreesX, 0F, center.X, center.Y);

		public static void SkewY(this IUGContext context, float degreesY)
			=> context.Skew(0F, degreesY);

		public static void SkewY(this IUGContext context, float degreesY, float centerX, float centerY)
			=> context.Skew(0F, degreesY, centerX, centerY);

		public static void SkewY(this IUGContext context, float degreesY, Vector2 center)
			=> context.Skew(0F, degreesY, center.X, center.Y);

		public static void Skew(this IUGContext context, float degreesX, float degreesY, Vector2 center)
			=> context.Skew(degreesX, degreesY, center.X, center.Y);

		#endregion

		#region Translate

		public static void TranslateX(this IUGContext context, float translateX)
			=> context.Translate(translateX, 0F);

		public static void TranslateY(this IUGContext context, float translateY)
			=> context.Translate(0F, translateY);

		public static void Translate(this IUGContext context, Vector2 translate)
			=> context.Translate(translate.X, translate.Y);

		#endregion
	}
}
