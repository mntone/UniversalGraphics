using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;

namespace UniversalGraphics.Win2D
{
#if WINDOWS_APP || WINDOWS_PHONE_APP
	using BuiltinMatrix3x2 = Microsoft.Graphics.Canvas.Numerics.Matrix3x2;

	internal static class Matrix3x2Helper
	{
		public static BuiltinMatrix3x2 CreateTranslate(float translateX, float translateY)
		{
			BuiltinMatrix3x2 result;
			result.M11 = 1F;
			result.M12 = 0F;
			result.M21 = 0F;
			result.M22 = 1F;
			result.M31 = translateX;
			result.M32 = translateY;
			return result;
		}

		public static BuiltinMatrix3x2 CreateScale(float scaleX, float scaleY)
		{
			BuiltinMatrix3x2 result;
			result.M11 = scaleX;
			result.M12 = 0F;
			result.M21 = 0F;
			result.M22 = scaleY;
			result.M31 = 0F;
			result.M32 = 0F;
			return result;
		}

		public static BuiltinMatrix3x2 CreateScale(float scaleX, float scaleY, float centerX, float centerY)
		{
			var tx = centerX * (1F - scaleX);
			var ty = centerY * (1F - scaleY);

			BuiltinMatrix3x2 result;
			result.M11 = scaleX;
			result.M12 = 0F;
			result.M21 = 0F;
			result.M22 = scaleY;
			result.M31 = tx;
			result.M32 = ty;
			return result;
		}

		public static BuiltinMatrix3x2 CreateSkew(float radiansX, float radiansY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);

			BuiltinMatrix3x2 result;
			result.M11 = 1F;
			result.M12 = skewY;
			result.M21 = skewX;
			result.M22 = 1F;
			result.M31 = 0F;
			result.M32 = 0F;
			return result;
		}

		public static BuiltinMatrix3x2 CreateSkew(float radiansX, float radiansY, float centerX, float centerY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			var tx = -centerY * skewX;
			var ty = -centerX * skewY;

			BuiltinMatrix3x2 result;
			result.M11 = 1F;
			result.M12 = skewY;
			result.M21 = skewX;
			result.M22 = 1F;
			result.M31 = tx;
			result.M32 = ty;
			return result;
		}

		public static BuiltinMatrix3x2 CreateRotation(float radians)
		{
			radians = (float)Math.IEEERemainder(radians, Math.PI * 2);

			float cos, sin;
			const float epsilon = 0.001F * (float)Math.PI / 180F;
			if (radians > -epsilon && radians < epsilon)
			{
				cos = 1F;
				sin = 0F;
			}
			else if (radians > Math.PI / 2 - epsilon && radians < Math.PI / 2 + epsilon)
			{
				cos = 0F;
				sin = 1F;
			}
			else if (radians < -Math.PI + epsilon || radians > Math.PI - epsilon)
			{
				cos = -1F;
				sin = 0F;
			}
			else if (radians > -Math.PI / 2 - epsilon && radians < -Math.PI / 2 + epsilon)
			{
				cos = 0F;
				sin = -1F;
			}
			else
			{
				cos = (float)Math.Cos(radians);
				sin = (float)Math.Sin(radians);
			}

			BuiltinMatrix3x2 result;
			result.M11 = cos;
			result.M12 = sin;
			result.M21 = -sin;
			result.M22 = cos;
			result.M31 = 0F;
			result.M32 = 0F;
			return result;
		}

		public static BuiltinMatrix3x2 CreateRotation(float radians, float centerX, float centerY)
		{
			radians = (float)Math.IEEERemainder(radians, 2.0 * Math.PI);

			float cos, sin;
			const float epsilon = 0.001F * (float)Math.PI / 180F;
			if (radians > -epsilon && radians < epsilon)
			{
				cos = 1F;
				sin = 0F;
			}
			else if (radians > Math.PI / 2 - epsilon && radians < Math.PI / 2 + epsilon)
			{
				cos = 0F;
				sin = 1F;
			}
			else if (radians < -Math.PI + epsilon || radians > Math.PI - epsilon)
			{
				cos = -1F;
				sin = 0F;
			}
			else if (radians > -Math.PI / 2 - epsilon && radians < -Math.PI / 2 + epsilon)
			{
				cos = 0F;
				sin = -1F;
			}
			else
			{
				cos = (float)Math.Cos(radians);
				sin = (float)Math.Sin(radians);
			}

			var tx = centerX * (1F - cos) + centerY * sin;
			var ty = centerY * (1F - cos) - centerX * sin;

			BuiltinMatrix3x2 result;
			result.M11 = cos;
			result.M12 = sin;
			result.M21 = -sin;
			result.M22 = cos;
			result.M31 = tx;
			result.M32 = ty;
			return result;
		}

		public static BuiltinMatrix3x2 Multiply(BuiltinMatrix3x2 a, BuiltinMatrix3x2 b)
		{
			BuiltinMatrix3x2 result;
			result.M11 = a.M11 * b.M11 + a.M12 * b.M21;
			result.M12 = a.M11 * b.M12 + a.M12 * b.M22;
			result.M21 = a.M21 * b.M11 + a.M22 * b.M21;
			result.M22 = a.M21 * b.M12 + a.M22 * b.M22;
			result.M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31;
			result.M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32;
			return result;
		}
	}
#endif

	public static class UGColorExtensions
	{
		public static Color ToWinRTColor(this UGColor color)
			=> Color.FromArgb(color.A, color.R, color.G, color.B);

		public static UGColor ToUGColor(this Color color)
			=> new UGColor(color.A, color.R, color.G, color.B);
	}

	public static class UGRectExtensions
	{
		public static Rect ToWinRTRect(this UGRect rect)
			=> new Rect(rect.X, rect.Y, rect.Width, rect.Height);

		public static UGRect ToUGRect(this Rect rect)
			=> new UGRect((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
	}

	internal static class UGStrokeStyleExtensions
	{
		public static CanvasCapStyle ToWin2DLineCap(this UGCapStyle capStyle)
		{
			switch (capStyle)
			{
				case UGCapStyle.Flat:
					return CanvasCapStyle.Flat;

				case UGCapStyle.Square:
					return CanvasCapStyle.Square;

				case UGCapStyle.Round:
					return CanvasCapStyle.Round;

				case UGCapStyle.Triangle:
					return CanvasCapStyle.Triangle;

				default:
					throw new NotSupportedException();
			}
		}

		public static CanvasLineJoin ToWin2DLineJoin(this UGLineJoin lineJoin)
		{
			switch (lineJoin)
			{
				case UGLineJoin.Miter:
					return CanvasLineJoin.Miter;

				case UGLineJoin.Bevel:
					return CanvasLineJoin.Bevel;

				case UGLineJoin.Round:
					return CanvasLineJoin.Round;

				case UGLineJoin.MiterOrBevel:
					return CanvasLineJoin.MiterOrBevel;

				default:
					throw new NotSupportedException();
			}
		}

		public static CanvasStrokeStyle ToWin2DStrokeStyle(this UGStrokeStyle strokeStyle)
		{
			var lineCap = strokeStyle.LineCap.ToWin2DLineCap();
			var retStrokeStyle = new CanvasStrokeStyle()
			{
				StartCap = lineCap,
				EndCap = lineCap,
				LineJoin = strokeStyle.LineJoin.ToWin2DLineJoin(),
				MiterLimit = strokeStyle.MiterLimit,
			};
			if (strokeStyle.DashStyle.Value != null)
			{
				retStrokeStyle.DashCap = lineCap;
				retStrokeStyle.CustomDashStyle = strokeStyle.DashStyle.Value;
				retStrokeStyle.DashOffset = strokeStyle.DashOffset;
			}
			return retStrokeStyle;
		}
	}

	internal static class UGEdgeBehaviorExtensions
	{
		public static CanvasEdgeBehavior ToWinRTEdgeBehavior(this UGEdgeBehavior edgeBehavior)
		{
			switch (edgeBehavior)
			{
				case UGEdgeBehavior.Clamp:
					return CanvasEdgeBehavior.Clamp;

				case UGEdgeBehavior.Wrap:
					return CanvasEdgeBehavior.Wrap;

				case UGEdgeBehavior.Mirror:
					return CanvasEdgeBehavior.Mirror;

				default:
					throw new NotSupportedException();
			}
		}

		public static UGEdgeBehavior ToUGEdgeBehavior(this CanvasEdgeBehavior edgeBehavior)
		{
			switch (edgeBehavior)
			{
				case CanvasEdgeBehavior.Clamp:
					return UGEdgeBehavior.Clamp;

				case CanvasEdgeBehavior.Wrap:
					return UGEdgeBehavior.Wrap;

				case CanvasEdgeBehavior.Mirror:
					return UGEdgeBehavior.Mirror;

				default:
					throw new NotSupportedException();
			}
		}
	}

	internal static class UGGradientStopExtensions
	{
		public static CanvasGradientStop[] ToWinRTGradientStops(this IEnumerable<UGGradientStop> stops)
			=> stops.Select(s => new CanvasGradientStop() { Color = s.Color.ToWinRTColor(), Position = s.Offset }).ToArray();

		public static UGGradientStop[] ToUGGradientStop(this CanvasGradientStop[] winrtStops)
			=> winrtStops.Select(s => new UGGradientStop(s.Color.ToUGColor(), s.Position)).ToArray();
	}
}
