using System.Numerics;
using System.Runtime.CompilerServices;

namespace UniversalGraphics
{
	public interface IUGPath : IUGObject
	{
		void Reset();
		void Close();

		void MoveTo(float startX, float startY);
		void LineTo(float endX, float endY);
		void CubicTo(float control1X, float control1Y, float control2X, float control2Y, float endX, float endY);
		void QuadTo(float controlX, float controlY, float endX, float endY);

		void AddCircle(float x, float y, float radius);
		void AddCircleInRectangle(float x, float y, float length);
		void AddEllipse(float x, float y, float radiusX, float radiusY);
		void AddEllipseInRectangle(float x, float y, float width, float height);
		void AddRectangle(float x, float y, float width, float height);
		void AddRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY);
	}

	public static class IUGPathExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MoveTo(this IUGPath path, Vector2 startPoint)
			=> path.MoveTo(startPoint.X, startPoint.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LineTo(this IUGPath path, Vector2 endPoint)
			=> path.LineTo(endPoint.X, endPoint.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CubicTo(this IUGPath path, Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
			=> path.CubicTo(controlPoint1.X, controlPoint1.Y, controlPoint2.X, controlPoint2.Y, endPoint.X, endPoint.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void QuadTo(this IUGPath path, Vector2 controlPoint, Vector2 endPoint)
			=> path.QuadTo(controlPoint.X, controlPoint.Y, endPoint.X, endPoint.Y);
	}
}
