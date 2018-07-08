using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;

namespace UniversalGraphics.GdiPlus
{
	internal static class MatrixHelper
	{
		public static Matrix CreateScale(float scaleX, float scaleY, float centerX, float centerY)
		{
			var tx = centerX * (1F - scaleX);
			var ty = centerY * (1F - scaleY);
			return new Matrix(scaleX, 0F, 0F, scaleY, tx, ty);
		}

		public static Matrix CreateSkew(float radiansX, float radiansY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			return new Matrix(1F, skewY, skewX, 1F, 0F, 0F);
		}

		public static Matrix CreateSkew(float radiansX, float radiansY, float centerX, float centerY)
		{
			var skewX = (float)Math.Tan(radiansX);
			var skewY = (float)Math.Tan(radiansY);
			var tx = -centerY * skewX;
			var ty = -centerX * skewY;
			return new Matrix(1F, skewY, skewX, 1F, tx, ty);
		}

		public static Matrix CreateRotation(float radians, float centerX, float centerY)
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
			return new Matrix(cos, sin, -sin, cos, tx, ty);
		}
	}

	public static class UGColorExtensions
	{
		public static Color ToGDIColor(this UGColor color)
			=> Color.FromArgb(color.ColorAsInt);

		public static UGColor ToUGColor(this Color color)
			=> new UGColor(color.A, color.R, color.G, color.B);
	}

	public static class Vector2Extensions
	{
		public static PointF ToGDIPointF(this Vector2 vector)
			=> new PointF(vector.X, vector.Y);

		public static Vector2 ToVector(this PointF point)
			=> new Vector2(point.X, point.Y);
	}

	public static class UGSizeExtensions
	{
		public static Size ToGDISize(this UGSize size)
			=> new Size((int)(size.Width +.5F), (int)(size.Height + .5F));

		public static UGSize ToUGSize(this Size size)
			=> new UGSize(size.Width, size.Height);
	}

	public static class UGRectExtensions
	{
		public static RectangleF ToGDIRect(this UGRect rect)
			=> new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);

		public static UGRect ToUGRect(this RectangleF rect)
			=> new UGRect(rect.X, rect.Y, rect.Width, rect.Height);

		public static UGRect ToUGRect(this Rectangle rect)
			=> new UGRect(rect.X, rect.Y, rect.Width, rect.Height);
	}

	internal static class UGStrokeStyleExtensions
	{
		public static LineCap ToGDILineCap(this UGCapStyle capStyle)
		{
			switch (capStyle)
			{
				case UGCapStyle.Flat:
					return LineCap.Flat;

				case UGCapStyle.Square:
					return LineCap.Square;

				case UGCapStyle.Round:
					return LineCap.Round;

				case UGCapStyle.Triangle:
					return LineCap.Triangle;

				default:
					throw new NotSupportedException();
			}
		}
		public static DashCap ToGDIDashCap(this UGCapStyle capStyle)
		{
			switch (capStyle)
			{
				case UGCapStyle.Flat:
				case UGCapStyle.Square:
					return DashCap.Flat;

				case UGCapStyle.Round:
					return DashCap.Round;

				case UGCapStyle.Triangle:
					return DashCap.Triangle;

				default:
					throw new NotSupportedException();
			}
		}

		public static LineJoin ToGDILineJoin(this UGLineJoin lineJoin)
		{
			switch (lineJoin)
			{
				case UGLineJoin.Miter:
					return LineJoin.Miter;

				case UGLineJoin.Bevel:
					return LineJoin.Bevel;

				case UGLineJoin.Round:
					return LineJoin.Round;

				case UGLineJoin.MiterOrBevel:
				default:
					throw new NotSupportedException();
			}
		}

		public static void SetStrokeStyle(this Pen pen, UGStrokeStyle strokeStyle)
		{
			var lineCap = strokeStyle.LineCap.ToGDILineCap();
			var dashCap = strokeStyle.LineCap.ToGDIDashCap();
			pen.SetLineCap(lineCap, lineCap, dashCap);
			pen.LineJoin = strokeStyle.LineJoin.ToGDILineJoin();
			pen.MiterLimit = strokeStyle.MiterLimit;
			if (strokeStyle.DashStyle.Value != null)
			{
				if (strokeStyle.LineCap != UGCapStyle.Flat)
				{
					var style = strokeStyle.DashStyle.Value;
					var count = style.Length;
					var copyStyle = new float[count];
					style.CopyTo(copyStyle, 0);
					for (var i = 0; i < count; i += 2)
					{
						copyStyle[i] += .999999F;
						copyStyle[i + 1] = Math.Max(style[i + 1] - .999999F, 0F);
					}
					if (!copyStyle.Where((v, i) => i % 2 == 1).All(v => v == 0))
					{
						pen.DashPattern = copyStyle;
						pen.DashOffset = strokeStyle.DashOffset - .5F;
					}
				}
				else
				{
					pen.DashPattern = strokeStyle.DashStyle.Value;
					pen.DashOffset = strokeStyle.DashOffset;
				}
			}
		}
	}

	internal static class UGEdgeBehaviorExtensions
	{
		public static WrapMode ToGDIWrapMode(this UGEdgeBehavior edgeBehavior)
		{
			switch (edgeBehavior)
			{
				case UGEdgeBehavior.Clamp:
					return WrapMode.Clamp;

				case UGEdgeBehavior.Wrap:
					return WrapMode.Tile;

				case UGEdgeBehavior.Mirror:
					return WrapMode.TileFlipXY;

				default:
					throw new NotSupportedException();
			}
		}

		public static UGEdgeBehavior ToUGEdgeBehavior(this WrapMode wrapMode)
		{
			switch (wrapMode)
			{
				case WrapMode.Clamp:
					return UGEdgeBehavior.Clamp;

				case WrapMode.Tile:
					return UGEdgeBehavior.Wrap;

				case WrapMode.TileFlipXY:
					return UGEdgeBehavior.Mirror;

				default:
					throw new NotSupportedException();
			}
		}
	}

	internal static class UGGradientStopExtensions
	{
		private const float EPSILON = 0.0001F;

		public static ColorBlend ToGDIColorBlend(this UGGradientStop[] ugStops)
		{
			var count = ugStops.Length;
			bool addStart = false, addEnd = false;
			if (ugStops[0].Offset > EPSILON)
			{
				addStart = true;
			}
			if (ugStops[count - 1].Offset < 1F - EPSILON)
			{
				addEnd = true;
			}

			var outCount = count;
			if (addStart) ++outCount;
			if (addEnd) ++outCount;

			var gdiStops = new ColorBlend(outCount);
			var outIdx = 0;
			if (addStart)
				ugStops[0].AddColorblend(ref gdiStops, ref outIdx, 0F);
			ugStops.AddColorBlend(ref gdiStops, ref outIdx);
			if (addEnd)
				ugStops[count - 1].AddColorblend(ref gdiStops, ref outIdx, 1F);
			return gdiStops;
		}
		
		public static ColorBlend ToGDIColorBlendWithEdgeBehavior(this UGGradientStop[] ugStops, UGEdgeBehavior edgeBehavior)
		{
			switch (edgeBehavior)
			{
				case UGEdgeBehavior.Clamp:
					return ugStops.ToGDIClampColorBlend();

				case UGEdgeBehavior.Wrap:
					return ugStops.ToGDIWrapColorBlend();

				case UGEdgeBehavior.Mirror:
					return ugStops.ToGDIMirrorColorBlend();

				default:
					throw new NotSupportedException();
			}
		}

		private static ColorBlend ToGDIClampColorBlend(this UGGradientStop[] ugStops)
		{
			var count = ugStops.Length;
			bool addEnd = false;
			if (ugStops[count - 1].Offset < 1F - EPSILON)
			{
				addEnd = true;
			}

			var outCount = count + 1;
			if (addEnd) ++outCount;

			var gdiStops = new ColorBlend(outCount);
			var outIdx = 0;
			ugStops[0].AddColorblend(ref gdiStops, ref outIdx, 0F);
			ugStops.AddColorBlend(ref gdiStops, ref outIdx, .5F, .5F);
			if (addEnd)
				ugStops[count - 1].AddColorblend(ref gdiStops, ref outIdx, 1F);
			return gdiStops;
		}

		private static ColorBlend ToGDIWrapColorBlend(this UGGradientStop[] ugStops)
		{
			var count = ugStops.Length;
			bool addStart = false, addEnd = false;
			if (ugStops[0].Offset > EPSILON)
			{
				addStart = true;
			}
			if (ugStops[count - 1].Offset < 1F - EPSILON)
			{
				addEnd = true;
			}

			var outCount = 2 * count;
			if (addStart) ++outCount;
			if (addEnd) ++outCount;

			var gdiStops = new ColorBlend(outCount);
			var outIdx = 0;
			if (addStart)
				ugStops[0].AddColorblend(ref gdiStops, ref outIdx, 0F);
			ugStops.AddColorBlend(ref gdiStops, ref outIdx, .5F);
			ugStops.AddColorBlend(ref gdiStops, ref outIdx, .5F, .5F);
			if (addEnd)
				ugStops[count - 1].AddColorblend(ref gdiStops, ref outIdx, 1F);
			return gdiStops;
		}

		private static ColorBlend ToGDIMirrorColorBlend(this UGGradientStop[] ugStops)
		{
			var count = ugStops.Length;
			bool addStart = false, addEnd = false;
			if (ugStops[count - 1].Offset > EPSILON)
			{
				addStart = true;
				addEnd = true;
			}

			var outCount = 2 * count;
			if (addStart) ++outCount;
			if (addEnd) ++outCount;

			var gdiStops = new ColorBlend(outCount);
			var outIdx = 0;
			if (addStart)
				ugStops[0].AddColorblend(ref gdiStops, ref outIdx, 0F);
			ugStops.AddColorBlendRev(ref gdiStops, ref outIdx, .5F);
			ugStops.AddColorBlend(ref gdiStops, ref outIdx, .5F, .5F);
			if (addEnd)
				ugStops[count - 1].AddColorblend(ref gdiStops, ref outIdx, 1F);
			return gdiStops;
		}

		private static void AddColorblend(this UGGradientStop ugStop, ref ColorBlend gdiStops, ref int outIdx, float offset)
		{
			gdiStops.Colors[outIdx] = ugStop.Color.ToGDIColor();
			gdiStops.Positions[outIdx] = offset;
			++outIdx;
		}

		private static void AddColorBlend(this IEnumerable<UGGradientStop> ugStops, ref ColorBlend gdiStops, ref int outIdx, float scale = 1F, float offset = 0F)
		{
			foreach (var ugStop in ugStops)
			{
				gdiStops.Colors[outIdx] = ugStop.Color.ToGDIColor();
				gdiStops.Positions[outIdx] = offset + scale * ugStop.Offset;
				++outIdx;
			}
		}

		private static void AddColorBlendRev(this IEnumerable<UGGradientStop> ugStops, ref ColorBlend gdiStops, ref int outIdx, float scale = 1F, float offset = 0F)
		{
			foreach (var ugStop in ugStops.Reverse())
			{
				gdiStops.Colors[outIdx] = ugStop.Color.ToGDIColor();
				gdiStops.Positions[outIdx] = offset + scale * (1F - ugStop.Offset);
				++outIdx;
			}
		}

		public static UGGradientStop[] ToUGGradientStop(this ColorBlend gdiStops)
		{
			var count = gdiStops.Colors.Length;
			var ugStops = new UGGradientStop[count];
			for (var i = 0; i < count; ++i)
			{
				var color = gdiStops.Colors[i];
				var position = gdiStops.Positions[i];
				ugStops[i] = new UGGradientStop(color.ToUGColor(), position);
			}
			return ugStops;
		}
	}

	internal static class UGTextAntialiasingExtensions
	{
		public static TextRenderingHint ToGDITextAntialiasing(this UGTextAntialiasing textAntialiasing)
		{
			switch (textAntialiasing)
			{
				case UGTextAntialiasing.Auto:
					return TextRenderingHint.SystemDefault;

				case UGTextAntialiasing.Aliased:
					return TextRenderingHint.SingleBitPerPixelGridFit;

				case UGTextAntialiasing.Antialiased:
					return TextRenderingHint.AntiAliasGridFit;

				case UGTextAntialiasing.SubpixelAntialiased:
					return TextRenderingHint.ClearTypeGridFit;

				default:
					throw new NotSupportedException();
			}
		}

		public static UGTextAntialiasing ToUGTextAntialiasing(this TextRenderingHint textAntialiasing)
		{
			switch (textAntialiasing)
			{
				case TextRenderingHint.SystemDefault:
					return UGTextAntialiasing.Auto;

				case TextRenderingHint.SingleBitPerPixelGridFit:
					return UGTextAntialiasing.SubpixelAntialiased;

				case TextRenderingHint.AntiAliasGridFit:
					return UGTextAntialiasing.Antialiased;

				case TextRenderingHint.ClearTypeGridFit:
					return UGTextAntialiasing.Aliased;

				default:
					throw new NotSupportedException();
			}
		}
	}
}
